using NotesLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesLib.Services;

public interface IUserService : IAsyncDisposable
{
    public Task<User> PostUser(string Name);

    public Task<User> PostUser(User User);

    public Task<User> GetUser(string UserName);
    public Task<User> GetUser(Guid id);

    public Task<IEnumerable<User>> GetAllUsers();

    public Task<User> UpdateUser(User User);
    public Task DeleteUser(User User);
}
