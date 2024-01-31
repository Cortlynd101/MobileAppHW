using NotesLib.Data;
using SQLite;

namespace NotesAPI.Initializers;

public class DatabaseInitialize 
{
    private string BaseDataDirectory { get; set; } =  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private string DatabaseName { get; set; } = "NotesAppDb";
    private Type[] tables { get; set; } = [typeof(User), typeof(Note)];
    public DatabaseInitialize(string? Path = null)
    {
        if (Path != null)
        {
            DatabaseName = Path;
        }
    }
    public async Task<SQLiteAsyncConnection> InitializeLocalDatabase()
    {
        var database = EstablishConnection();

        await database.CreateTablesAsync(CreateFlags.None, tables);

        return database;
    }

    public SQLiteAsyncConnection EstablishConnection()
    {
        if (!Directory.Exists(BaseDataDirectory)) Directory.CreateDirectory(BaseDataDirectory);

        var database = new SQLiteAsyncConnection(Path.Combine(BaseDataDirectory, DatabaseName));

        return database;
    }

    public async Task<SQLiteAsyncConnection> RestartDatabase()
    {
        var database = EstablishConnection();
        await database.CreateTablesAsync(CreateFlags.None, tables);
        await database.DeleteAllAsync<User>();
        await database.DeleteAllAsync<Note>();
        return database;
    }
}
