//using FluentAssertions;
//using MAUINotes.Classes;
//using MAUINotes.Initializers;
//using MAUINotes.Services;
//using Newtonsoft.Json;
//using NotesAPI.Initializers;
//using NotesAPI.Services;
//using NotesLib.Data;
//using NotesTests.Initializers;
//using System.Net.Http.Json;
//using System.Net.Http.Formatting;

//namespace NotesTests;

//public class SyncTests : IClassFixture<NoteApiFactory>, IDisposable
//{
//    public HttpClient httpClient { get; set; }
//    public NoteApiFactory NoteApiFactory { get; set; }
//	private static string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
//	public SyncTests(NoteApiFactory factory)
//    {
//        httpClient = factory.CreateDefaultClient();
//        NoteApiFactory = factory;
//    }
//    [Fact]
//    public async Task MauiUsersSyncToDatabaseIfMauiUserWasntInApi()
//    {
//		//Arrange
//		string MauiPath = "11";
//        await TestInitializer.RestartDatabases(MauiPath, NoteApiFactory.DatabaseName);
//        MauiUserService mauiUserService = new MauiUserService(new LocalDatabaseInitializer(MauiPath, FilePath));
//        ApiUserService apiUserService = new ApiUserService(new DatabaseInitialize(NoteApiFactory.DatabaseName));
//        Sync sync = new Sync(MauiPath, FilePath);


//        //Act
//        await mauiUserService.PostUser("Carlos");
//        await sync.SyncUsers(httpClient);
//        var result = await apiUserService.GetUser("Carlos");

//        //Assert
//        result.Should().NotBeNull();

//        //Clean
//        await mauiUserService.DisposeAsync();
//        await apiUserService.DisposeAsync();
//        await sync.DisposeAsync();
//        File.Delete(Path.Combine(FilePath, MauiPath));
//    }

//    [Fact]
//    public async Task ApiUsersSyncToMauiIfApiUserWasntInMaui()
//    {
//        //Arrange
//        string MauiPath = "13";
//        await TestInitializer.RestartDatabases(MauiPath, NoteApiFactory.DatabaseName);
//        MauiUserService mauiUserService = new MauiUserService(new LocalDatabaseInitializer(MauiPath, FilePath));
//        ApiUserService apiUserService = new ApiUserService(new DatabaseInitialize(NoteApiFactory.DatabaseName));
//        Sync sync = new Sync(MauiPath, FilePath);


//        //Act
//        var user = await apiUserService.PostUser("Chad");
//        await sync.SyncUsers(httpClient);
//        var result = await mauiUserService.GetUser(user.Id);

//        //Assert
//        result.Should().NotBeNull();

//        //Clean
//		await mauiUserService.DisposeAsync();
//		await apiUserService.DisposeAsync();
//		await sync.DisposeAsync();
//		File.Delete(Path.Combine(FilePath, MauiPath));
//	}

//    [Fact]
//    public async Task ChaingingAUserInMauiAfterItGotSyncedToTheApiSyncsAgain()
//    {
//        //Arrange
//        string MauiPath = "1";
//        await TestInitializer.RestartDatabases(MauiPath, NoteApiFactory.DatabaseName);
//        MauiUserService mauiUserService = new MauiUserService(new LocalDatabaseInitializer(MauiPath, FilePath));
//        ApiUserService apiUserService = new ApiUserService(new DatabaseInitialize(NoteApiFactory.DatabaseName));
//        Sync sync = new Sync(MauiPath, FilePath);

//        //Act
//        var user = await mauiUserService.PostUser("Chad");
//        await sync.SyncUsers(httpClient);
//        var updatedUser = new User {Id = user.Id, Name = "Chadette", LastSync = DateTime.Now };
//        var updated = await mauiUserService.UpdateUser(updatedUser);
//        await sync.SyncUsers(httpClient);
//        var check = await apiUserService.GetUser(updated.Id);

//        //Assert
//        check.Equals(updated).Should().BeTrue();

//		//Clean
//		await mauiUserService.DisposeAsync();
//		await apiUserService.DisposeAsync();
//		await sync.DisposeAsync();
//		File.Delete(Path.Combine(FilePath, MauiPath));
//	}

//    [Fact]
//    public async Task ChaingingAUserInApiAfterItGotSyncedToTheMauiSyncsAgain()
//    {
//        //Arrange
//        string MauiPath = "15";
//        await TestInitializer.RestartDatabases(MauiPath, NoteApiFactory.DatabaseName);
//        MauiUserService mauiUserService = new MauiUserService(new LocalDatabaseInitializer(MauiPath, FilePath));
//        ApiUserService apiUserService = new ApiUserService(new DatabaseInitialize(NoteApiFactory.DatabaseName));
//        Sync sync = new Sync(MauiPath, FilePath);

//        //Act
//        var user = await apiUserService.PostUser("Chad");
//        await sync.SyncUsers(httpClient);
//        var updatedUser = new User { Id = user.Id, Name = "Chadette", LastSync = DateTime.Now };
//        var updated = await apiUserService.UpdateUser(updatedUser);
//        await sync.SyncUsers(httpClient);
//        var check = await mauiUserService.GetUser(updated.Id);

//        //Assert
//        check.Equals(updated).Should().BeTrue();

//		//Clean
//		await mauiUserService.DisposeAsync();
//		await apiUserService.DisposeAsync();
//		await sync.DisposeAsync();
//		File.Delete(Path.Combine(FilePath, MauiPath));
//	}

//	public void Dispose()
//	{
//        return;
//	}
//}