using MAUINotes.Initializers;
using NotesLib.Data;
using NotesLib.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MAUINotes.Services
{
    public class MauiUserService : IUserService
	{
        public SQLiteAsyncConnection dbContext { get; set; }
		public MauiUserService(LocalDatabaseInitializer dbInitialize)
		{
			Task.Run(() => dbInitialize.InitializeLocalDatabase()).Wait();
			dbContext = dbInitialize.EstablishConnection();
		}
		public async Task<User> GetUser(string UserName)
		{
			return await dbContext.FindAsync<User>(x => x.Name == UserName);
		}

		public async Task<User> PostUser(string Name)
		{
			var User = new User {Id=Guid.NewGuid(), Name = Name, LastSync = DateTime.Now };
			await dbContext.InsertAsync(User);
			return User;
		}

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await dbContext.Table<User>().Where(x => x.Name != null).ToListAsync();
        }

        public async Task<User> PostUser(User User)
        {
            await dbContext.InsertOrReplaceAsync(User);
            return User;
        }

        public async Task<User> UpdateUser(User User)
        {
            var user = await dbContext.FindAsync<User>(x => x.Id == User.Id);
            if (user == null)
            {
                throw new Exception("Can't update a user that does not exist");
            }
            user.Name = User.Name;
            user.LastSync = User.LastSync;
            await dbContext.UpdateAsync(user);
            return user;
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await dbContext.Table<User>().Where(x => true).ToListAsync();
            var users = await dbContext.GetAsync<User>(x => true);
            return await dbContext.FindAsync<User> (x => x.Id == id);
        }

		public async ValueTask DisposeAsync()
		{
			await dbContext.CloseAsync();
		}

		public async Task DeleteUser(User User)
		{
            var user = User.Name = null;
            await dbContext.UpdateAsync(user);
		}
	}
}
