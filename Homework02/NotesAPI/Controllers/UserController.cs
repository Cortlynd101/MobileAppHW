using Microsoft.AspNetCore.Mvc;
using NotesLib.Data;
using NotesLib.Services;

namespace NotesAPI.Controllers
{
    public class UserController(IUserService userService)
    {
        [HttpGet("/getUser/{name}")]
        public async Task<User> GetUser(string name)
        {
            return await userService.GetUser(name);
        }

        [HttpPost("/postUser/{name}")]
        public Task<User> PostUser(string name)
        {
            return userService.PostUser(name);
        }

        [HttpPost("/postUser")]
        public Task<User> PostUser([FromBody] User user)
        {
            return userService.PostUser(user);
        }

        [HttpGet("/getAllUsers")]
        public Task<IEnumerable<User>> GetUsers()
        {
            return userService.GetAllUsers();
        }

        [HttpPatch("/patchUser")]
        public async Task<User> PatchUser([FromBody] User user)
        {
            return await userService.UpdateUser(user);
        }
    }
}
