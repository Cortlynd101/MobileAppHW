using FluentAssertions;
using MAUINotes.Classes;
using MAUINotes.Initializers;
using MAUINotes.Services;
using Newtonsoft.Json;
using NotesAPI.Initializers;
using NotesAPI.Services;
using NotesLib.Data;
using NotesTests.Initializers;
using System.Net.Http.Json;
using System.Net.Http.Formatting;

namespace NotesTests
{
    public class ExampleTests : IClassFixture<NoteApiFactory>
    {
        public HttpClient httpClient { get; set; }
        public NoteApiFactory NoteApiFactory { get; set; }
        private static string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public ExampleTests(NoteApiFactory factory)
        {
            httpClient = factory.CreateDefaultClient();
            NoteApiFactory = factory;
        }

        [Fact]
        public async Task ExampleTestUserAdded()
        {
            //Arrange
            string MauiPath = "16";
            await TestInitializer.RestartDatabases(MauiPath, NoteApiFactory.DatabaseName);
            MauiUserService mauiUserService = new MauiUserService(new LocalDatabaseInitializer(MauiPath, FilePath));
            Sync sync = new Sync(MauiPath, FilePath);

            //Act
            await mauiUserService.PostUser("Cortlynd");
            var result = await sync.GetUserDifferences(httpClient);

            //Assert
            result.Should().HaveCountGreaterThan(0);

            //Clean
            await mauiUserService.DisposeAsync();
            await sync.DisposeAsync();
            File.Delete(Path.Combine(FilePath, MauiPath));
        }
    }
}
