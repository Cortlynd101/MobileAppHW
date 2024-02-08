using MAUINotes.Services;
using NotesLib.Data;
using System.Net.Http.Json;
using static SQLite.SQLite3;

namespace MAUINotes.Classes;

public class Sync : IAsyncDisposable
{
    HttpClient HttpClient { get; set; } = new HttpClient() { BaseAddress = new Uri("https://localhost:7286") };
    MauiUserService MauiUserService { get; set; }
    MauiNoteService MauiNoteService { get; set; }

    public Sync(string? Path = null, string? DirPath = null)
    {
        MauiUserService = new MauiUserService(new Initializers.LocalDatabaseInitializer(Path, DirPath));
        MauiNoteService = new MauiNoteService(new Initializers.LocalDatabaseInitializer(Path, DirPath));
    }


    public  async Task LocalToAPI(HttpClient _httpClient = null)
    {
        await SyncUsers(_httpClient);
        await SyncNotes(_httpClient);
    }

    public  async Task SyncNotes(HttpClient _httpClient)
    {
        var differences = await GetNoteDifferences(_httpClient);
    }

    public  async Task SyncUsers(HttpClient _httpClient=null)
    {
        var httpClient = HttpClient;
        if (_httpClient is not null)
        {
            httpClient = _httpClient;
        }
        
        var differences = await GetUserDifferences(httpClient);
        foreach (var difference in differences)
        {
            if (difference.Maui is null)
            {
                var result = await MauiUserService.PostUser(difference.Api);
                if(result is null)
                {
                    throw new Exception("Error moving new items to maui from api");
                }
            }
            else if (difference.Api is null)
            {
                var result = await httpClient.PostAsJsonAsync($"/postUser", difference.Maui);

                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception("Error moving new items to api from maui");
                }
                return;
            }
            else if (difference.Api.LastSync < difference.Maui.LastSync)
            {
                var response = await httpClient.PatchAsJsonAsync("/PatchUser", difference.Maui);
                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception("HERES THE BREAK! UPDATED ROW LOCALY GOING TO API");
                }
                return;
            }
            else
            {
                var result = await MauiUserService.PostUser(difference.Api);
                if(result == null)
                {
                    throw new Exception("Error moving api data to maui");
                }
            }
        }
    }

    public  async Task<IEnumerable<(User Maui, User Api)>> GetUserDifferences(HttpClient _httpClient = null)
    {
        var httpClient = HttpClient;
        if(_httpClient is not null)
        {
            httpClient = _httpClient; 
        }

        var apiUsers = await httpClient.GetFromJsonAsync<IEnumerable<User>>("/getAllUsers");
        var mauiUsers = await MauiUserService.GetAllUsers();

        apiUsers = apiUsers.OrderBy(x => x.Id).ToList();
        mauiUsers = mauiUsers.OrderBy(x => x.Id).ToList();

        HashSet<(Guid, DateTime)> apiUsersHash = new HashSet<(Guid, DateTime)>(apiUsers.Select(x => (x.Id, x.LastSync)));
        HashSet<(Guid, DateTime)> mauiUsersHash = new HashSet<(Guid, DateTime)>(mauiUsers.Select(x => (x.Id, x.LastSync)));

        var mauiDifferences = mauiUsers
            .Where(x => !apiUsersHash
                .Contains((x.Id, x.LastSync)))
            .Select(x => (Maui: x, Api: apiUsers.FirstOrDefault(y => y.Id == x.Id)))
            .ToList();

        var apiDifferences = apiUsers
            .Where(x => !mauiUsersHash
                .Contains((x.Id, x.LastSync)))
            .Select(x => (Maui: mauiUsers.FirstOrDefault(y => y.Id == x.Id), Api: x))
            .ToList();

        return mauiDifferences.Union<(User Maui, User Api)>(apiDifferences).ToList();
    }

    public  async Task<IEnumerable<(Note Maui, Note Api)>> GetNoteDifferences(HttpClient _httpClient = null)
    {
        var httpClient = HttpClient;
        if (_httpClient is not null)
        {
            httpClient = _httpClient;
        }

        var apiNotes = await httpClient.GetFromJsonAsync<IEnumerable<Note>>("/getNotes");
        var mauiNotes = await MauiNoteService.GetAllNotes();

        apiNotes = apiNotes.OrderBy(x => x.Id).ToList();
        mauiNotes = mauiNotes.OrderBy(x => x.Id).ToList();

        HashSet<(Guid, DateTime)> apiNoteHash = new HashSet<(Guid, DateTime)>(apiNotes.Select(x => (x.Id, x.LastSync)));
        HashSet<(Guid, DateTime)> mauiNoteHash = new HashSet<(Guid, DateTime)>(mauiNotes.Select(x => (x.Id, x.LastSync)));

        var mauiDifferences = mauiNotes
            .Where(x => !apiNoteHash
                .Contains((x.Id, x.LastSync)))
            .Select(x => (Maui:x, Api:apiNotes.FirstOrDefault(y => y.Id == x.Id)))
            .ToList();

        var apiDifferences = apiNotes
            .Where(x => !mauiNoteHash
                .Contains((x.Id, x.LastSync)))
            .Select(x => (Maui: mauiNotes.FirstOrDefault(y => y.Id == x.Id), Api:x))
            .ToList();

        return mauiDifferences.Union(apiDifferences).ToList();
    }

	public async ValueTask DisposeAsync()
	{
		await MauiUserService.DisposeAsync();
        //await MauiNoteService.DisposeAsync()
	}
}
