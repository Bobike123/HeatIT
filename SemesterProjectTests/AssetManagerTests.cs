using Xunit;
using SemesterProject;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using System.Collections.Generic;

public class AssetManagerTests
{
    private readonly MockFileSystem _mockFileSystem;
    private readonly AssetManager _assetManager;

    public AssetManagerTests()
    {
        _mockFileSystem = new MockFileSystem();
        _assetManager = new AssetManager();
        // Override static PathJson to use mock file system
        AssetManager.PathJson = _mockFileSystem.Path.Combine(_mockFileSystem.Directory.GetCurrentDirectory(), "AssetManager", "units.json");
    }

    [Fact]
    public void Save_ShouldWriteJsonToFile()
    {
        // Arrange
        string currentDirectory = _mockFileSystem.Directory.GetCurrentDirectory();
        string resultDirectory = _mockFileSystem.Path.Combine(currentDirectory, "ResultDataManager");
        _mockFileSystem.Directory.CreateDirectory(resultDirectory);

        string expectedPath = _mockFileSystem.Path.Combine(resultDirectory, "SavedJsonData.json");
        string expectedJson = JsonSerializer.Serialize(AssetManager.productionUnits);

        // Act
        _assetManager.Save();

        // Assert
        Assert.True(_mockFileSystem.FileExists(expectedPath));
        string actualJson = _mockFileSystem.File.ReadAllText(expectedPath);
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void Load_ShouldDeserializeJsonFromFile()
    {
        // Arrange
        string currentDirectory = _mockFileSystem.Directory.GetCurrentDirectory();
        string assetDirectory = _mockFileSystem.Path.Combine(currentDirectory, "AssetManager");
        _mockFileSystem.Directory.CreateDirectory(assetDirectory);

        string pathJson = _mockFileSystem.Path.Combine(assetDirectory, "units.json");
        string jsonContent = JsonSerializer.Serialize(new List<ProductionUnit>
        {
            new ProductionUnit("Test1", "1.0", "0", "100", "50", "testFuel")
        });
        _mockFileSystem.AddFile(pathJson, new MockFileData(jsonContent));

        // Act
        _assetManager.Load();

        // Assert
        Assert.Single(AssetManager.productionUnits);
        Assert.Equal("Test1", AssetManager.productionUnits[0].Name);
    }

    [Fact]
    public void Exist_ShouldReturnTrueIfFileExists()
    {
        // Arrange
        string currentDirectory = _mockFileSystem.Directory.GetCurrentDirectory();
        string assetDirectory = _mockFileSystem.Path.Combine(currentDirectory, "AssetManager");
        _mockFileSystem.Directory.CreateDirectory(assetDirectory);

        string pathJson = _mockFileSystem.Path.Combine(assetDirectory, "units.json");
        _mockFileSystem.AddFile(pathJson, new MockFileData(""));

        // Act
        bool exists = AssetManager.Exist();

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public void Exist_ShouldCreateFileIfNotExists()
    {
        // Arrange
        string currentDirectory = _mockFileSystem.Directory.GetCurrentDirectory();
        string assetDirectory = _mockFileSystem.Path.Combine(currentDirectory, "AssetManager");
        _mockFileSystem.Directory.CreateDirectory(assetDirectory);

        string pathJson = _mockFileSystem.Path.Combine(assetDirectory, "units.json");

        // Act
        bool exists = AssetManager.Exist();

        // Assert
        Assert.True(exists);
        Assert.True(_mockFileSystem.FileExists(pathJson));
    }
}
