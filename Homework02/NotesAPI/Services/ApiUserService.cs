using NotesAPI.Initializers;
using NotesLib.Data;
using NotesLib.Services;
using SQLite;

namespace NotesAPI.Services;

public class ApiUserService : IUserService
{
    private SQLiteAsyncConnection dbContext { get; set; }
    public static DatabaseInitialize DbInitialize { get; } = new();

	public ApiUserService(DatabaseInitialize dbInitialize)
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
        var newUser = new User()
        {
            Id = User.Id,
            Name = User.Name,
            LastSync = DateTime.Now,
        };
        await dbContext.InsertAsync(newUser);
        return User;
    }

    public async Task<User> UpdateUser(User User)
    {
        //var users = await dbContext.Table<User>().Where(x => true).ToListAsync();
        //var user = await dbContext.FindAsync<User>(x => x.Id == User.Id);
        //if (user == null)
        //{
        //    throw new Exception("Can't update a user that does not exist");
        //}
        //user.Name = User.Name;
        //user.LastSync = User.LastSync;
        await dbContext.UpdateAsync(User);
        return User;
    }

    public async Task<User> GetUser(Guid id)
    {
        return await dbContext.FindAsync<User>(x => x.Id == id);
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
