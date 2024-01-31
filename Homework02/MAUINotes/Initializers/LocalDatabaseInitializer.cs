using NotesLib.Data;
using SQLite;

namespace MAUINotes.Initializers
{
    public  class LocalDatabaseInitializer
    {
        private string BaseDataDirectory { get; set; } 
        private string DatabaseName { get; set; } = "local";
        private  Type[] tables { get; set; } = [typeof(User), typeof(Note)];
        public LocalDatabaseInitializer(string? Path = null, string? DirectPath = null)
        {
            if (Path != null)
            {
                DatabaseName = Path;
            }
            if(DirectPath != null)
            {
                BaseDataDirectory = DirectPath;
            }
            else
            {
				BaseDataDirectory = FileSystem.Current.AppDataDirectory;
			}
        }
        public  async Task<SQLiteAsyncConnection> InitializeLocalDatabase()
        {
            var database = EstablishConnection();

            await database.CreateTablesAsync(CreateFlags.None, tables);

            return database;
        }

        public  SQLiteAsyncConnection EstablishConnection() 
        {
            if (!Directory.Exists(BaseDataDirectory)) Directory.CreateDirectory(BaseDataDirectory);

            var database = new SQLiteAsyncConnection(Path.Combine(BaseDataDirectory, DatabaseName));

            return database;
        }

        public  async Task<SQLiteAsyncConnection> RestartDatabase()
        {
            var database = EstablishConnection();
            await database.CreateTablesAsync(CreateFlags.None, tables);
            await database.DeleteAllAsync<User>();
            await database.DeleteAllAsync<Note>();
            return database;
        }
    }
}
