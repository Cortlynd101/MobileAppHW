//using FluentAssertions;
//using MAUINotes.Classes;
//using MAUINotes.Initializers;
//using MAUINotes.Services;
//using NotesAPI.Initializers;
//using NotesAPI.Services;
//using NotesLib.Data;
//using NotesLib.Services;
//using NotesTests.Initializers;

//namespace NotesTests;

//public class DifferenceTests : IClassFixture<NoteApiFactory>
//{
//    public HttpClient httpClient { get; set; }
//    public NoteApiFactory NoteApiFactory { get; set; }
//	private static string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
//	public DifferenceTests(NoteApiFactory factory)
//    {
//        httpClient = factory.CreateDefaultClient();
//        NoteApiFactory = factory;
//    }

//    [Fact]
//    public async Task DifferenceInUsersIsFound()
//    {
//        //Arrange
//        string MauiPath = "3";
//        await TestInitializer.RestartDatabases(MauiPath, NoteApiFactory.DatabaseName);
//        MauiUserService mauiUserService = new MauiUserService(new LocalDatabaseInitializer(MauiPath, FilePath));
//        Sync sync = new Sync(MauiPath, FilePath);

//        //Act
//        await mauiUserService.PostUser("Cort");
//        var result = await sync.GetUserDifferences(httpClient);

//        //Assert
//        result.Should().HaveCountGreaterThan(0);

//		//Clean
//		await mauiUserService.DisposeAsync();
//		await sync.DisposeAsync();
//		File.Delete(Path.Combine(FilePath, MauiPath));
//	}

   

//    [Fact]
//    public async Task DifferenceInNotesIsFound()
//    {
//        //a=Arrange
//        string MauiPath = "5";
//        await TestInitializer.RestartDatabases(MauiPath, NoteApiFactory.DatabaseName);
//        MauiNoteService mauiNoteService = new MauiNoteService(new LocalDatabaseInitializer(MauiPath, FilePath));
//        Sync sync = new Sync(MauiPath, FilePath);

//        //Act
//        Note newNote = new Note();
//        await mauiNoteService.PostNote(newNote);
//        var result = await sync.GetNoteDifferences(httpClient);

//        //Assert
//        result.Should().HaveCountGreaterThan(0);

//		//Clean
//		await mauiNoteService.DisposeAsync();
//		await sync.DisposeAsync();
//		File.Delete(Path.Combine(FilePath, MauiPath));
//	}

//    [Fact]
//    public async Task DifferenceInUsersIsFoundWhenApiChangesFirst()
//    {
//        //Arrange
//        string MauiPath = "7";
//        await TestInitializer.RestartDatabases(MauiPath, NoteApiFactory.DatabaseName);
//        ApiUserService apiUserService = new ApiUserService(new DatabaseInitialize(NoteApiFactory.DatabaseName));
//        Sync sync = new Sync(MauiPath, FilePath);

//        //Act
//        await apiUserService.PostUser("Carlos");
//        var result = await sync.GetUserDifferences(httpClient);

//        //Assert
//        result.Should().HaveCountGreaterThan(0);

//		//Clean
//		await apiUserService.DisposeAsync();
//		await sync.DisposeAsync();
//		File.Delete(Path.Combine(FilePath, MauiPath));
//	}

//    [Fact]
//    public async Task DifferenceInNotesIsFoundWhenApiChangesFirst()
//    {
//        //Arrange
//        string MauiPath = "9";
//        await TestInitializer.RestartDatabases(MauiPath, NoteApiFactory.DatabaseName);
//        ApiNoteService apiNoteService = new ApiNoteService(new DatabaseInitialize(NoteApiFactory.DatabaseName));
//        Sync sync = new Sync(MauiPath, FilePath);

//        //Act
//        Note newNote = new Note();
//        await apiNoteService.PostNote(newNote);
//        var result = await sync.GetNoteDifferences(httpClient);

//        //Assert
//        result.Should().HaveCountGreaterThan(0);

//		//Clean
//		await apiNoteService.DisposeAsync();
//		await sync.DisposeAsync();
//		File.Delete(Path.Combine(FilePath, MauiPath));
//	}
//}
