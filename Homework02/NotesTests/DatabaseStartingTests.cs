using FluentAssertions;
using MAUINotes.Classes;
using MAUINotes.Initializers;
using MAUINotes.Services;
using NotesAPI.Initializers;
using NotesAPI.Services;
using NotesLib.Data;
using NotesTests.Initializers;

namespace NotesTests;

public class DatabaseStartingTests
{
	private static string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
	[Fact]
    public async Task MauiDoesntUpdateApiDatabase()
    {
        //Arrange
        string MauiPath = "17";
        string ApiPath = "18";
        await TestInitializer.RestartDatabases(MauiPath, ApiPath);
        ApiUserService apiUserService = new ApiUserService(new DatabaseInitialize(ApiPath));
        MauiUserService mauiUserService = new MauiUserService(new LocalDatabaseInitializer(MauiPath, FilePath));

        //Act
        var user = await mauiUserService.PostUser("Carlos");
        var result = await apiUserService.GetUser(user.Id);

        //Assert
        result.Should().BeNull();

		//Clean
		await mauiUserService.DisposeAsync();
		await apiUserService.DisposeAsync();
		File.Delete(Path.Combine(FilePath, MauiPath));
		File.Delete(Path.Combine(FilePath, ApiPath));
	}

    [Fact]

    public async Task ApiDoesntUpdateMauiDatabase()
    {
        //Arrange
        string MauiPath = "19";
        string ApiPath = "20";
        await TestInitializer.RestartDatabases(MauiPath, ApiPath);
        ApiUserService apiUserService = new ApiUserService(new DatabaseInitialize(ApiPath));
        MauiUserService mauiUserService = new MauiUserService(new LocalDatabaseInitializer(MauiPath, FilePath));

        //Act
        var user = await mauiUserService.PostUser("Carlos");
        var result = await apiUserService.GetUser(user.Id);

        //Assert
        result.Should().BeNull();

		//Clean
		await mauiUserService.DisposeAsync();
		await apiUserService.DisposeAsync();
		File.Delete(Path.Combine(FilePath, MauiPath));
		File.Delete(Path.Combine(FilePath, ApiPath));
	}
}