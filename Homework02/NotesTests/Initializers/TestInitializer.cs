using MAUINotes.Initializers;
using NotesAPI.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesTests.Initializers;

public static class TestInitializer
{
    private static string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

	public static async Task RestartDatabases(string MauiPath, string ApiPath)
    {
        await new LocalDatabaseInitializer(MauiPath, FilePath).RestartDatabase();
        await new DatabaseInitialize(ApiPath).RestartDatabase();
    }

    public static async Task DeleteDatabases(string Maui, string Api)
    {

    }
}
