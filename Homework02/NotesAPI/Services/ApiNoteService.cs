using NotesAPI.Initializers;
using NotesLib.Data;
using NotesLib.Services;
using SQLite;

namespace NotesAPI.Services;

public class ApiNoteService : INotesService
{
    private SQLiteAsyncConnection dbContext { get; set; }
	public ApiNoteService(DatabaseInitialize dbInitialize)
	{
		Task.Run(() => dbInitialize.InitializeLocalDatabase()).Wait();
		dbContext = dbInitialize.EstablishConnection();
	}

	public Task DeleteNote(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Note>> GetAllNotes()
    {
        return await dbContext.Table<Note>().ToListAsync();
    }

    public async Task<Note> GetNote(Guid Id)
    { 
        return await dbContext.GetAsync<Note>(x => x.Id == Id);    
    }

    public async Task<Note> PostNote(Note note)
    {
        await dbContext.InsertAsync(note);
        return note;
    }

    public Task UpdateNote(Note note)
    {
        throw new NotImplementedException();
    }

	public async ValueTask DisposeAsync()
	{
		await dbContext.CloseAsync();
	}
}
