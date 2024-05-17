using Xunit;
using SemesterProject;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using System.Collections.Generic;

public class ReadFileTests
{
    [Fact]
    public void Save_ShouldWriteJsonToFile()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        string currentDirectory = mockFileSystem.Directory.GetCurrentDirectory();
        string resultDirectory = mockFileSystem.Path.Combine(currentDirectory, "ResultDataManager");
        mockFileSystem.Directory.CreateDirectory(resultDirectory);

        var readFile = new AssetManager();
        string expectedPath = mockFileSystem.Path.Combine(resultDirectory, "SavedJsonData.json");
        string expectedJson = JsonSerializer.Serialize(AssetManager.productionUnits);

        // Act
        readFile.Save();

        // Assert
        Assert.True(mockFileSystem.FileExists(expectedPath));
        string actualJson = mockFileSystem.File.ReadAllText(expectedPath);
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void Load_ShouldDeserializeJsonFromFile()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        string currentDirectory = mockFileSystem.Directory.GetCurrentDirectory();
        string assetDirectory = mockFileSystem.Path.Combine(currentDirectory, "AssetManager");
        mockFileSystem.Directory.CreateDirectory(assetDirectory);

        string pathJson = mockFileSystem.Path.Combine(assetDirectory, "units.json");
        string jsonContent = JsonSerializer.Serialize(new List<ProductionUnit>
        {
            new ProductionUnit("Test1", "1.0", "0", "100", "50", "testFuel")
        });
        mockFileSystem.AddFile(pathJson, new MockFileData(jsonContent));

        var readFile = new AssetManager();

        // Act
        readFile.Load();

        // Assert
        Assert.Single(AssetManager.productionUnits);
        Assert.Equal("Test1", AssetManager.productionUnits[0].Name);
    }

    [Fact]
    public void Exist_ShouldReturnTrueIfFileExists()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        string currentDirectory = mockFileSystem.Directory.GetCurrentDirectory();
        string assetDirectory = mockFileSystem.Path.Combine(currentDirectory, "AssetManager");
        mockFileSystem.Directory.CreateDirectory(assetDirectory);

        string pathJson = mockFileSystem.Path.Combine(assetDirectory, "units.json");
        mockFileSystem.AddFile(pathJson, new MockFileData(""));

        // Act
        bool exists = AssetManager.Exist();

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void Exist_ShouldCreateFileIfNotExists()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        string currentDirectory = mockFileSystem.Directory.GetCurrentDirectory();
        string assetDirectory = mockFileSystem.Path.Combine(currentDirectory, "AssetManager");
        mockFileSystem.Directory.CreateDirectory(assetDirectory);

        string pathJson = mockFileSystem.Path.Combine(assetDirectory, "units.json");

        // Act
        bool exists = AssetManager.Exist();

        // Assert
        Assert.True(exists);
        Assert.True(mockFileSystem.FileExists(pathJson));
    }
}
