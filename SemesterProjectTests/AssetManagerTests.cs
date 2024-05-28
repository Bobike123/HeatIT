using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using SemesterProject;


public class AssetManagerTests
{
    [Fact]
    public void PathJson_ShouldBeCorrect()
    {
        // Arrange
        var expectedPath = Path.Combine(Directory.GetCurrentDirectory(), "AssetManager", "units.json");

        // Act
        var actualPath = AssetManager.PathJson;

        // Assert
        Assert.Equal(expectedPath, actualPath);
    }

    [Fact]
    public void ProductionUnits_ShouldContainCorrectUnits()
    {
        // Arrange
        var assetManager = new AssetManager();
        var expectedUnits = new List<ProductionUnit>
        {
            new ProductionUnit("GB", "5.0", "0", "500", "215", "gas"),
            new ProductionUnit("OB", "4.0", "0", "700", "265", "oil"),
            new ProductionUnit("GM", "3.6", "2.7", "1100", "640", "gas"),
            new ProductionUnit("EK", "8.0", "-8.0", "50", "0", "electricity"),
        };

        // Act
        var actualUnits = assetManager.productionUnits;

        // Assert
        Assert.Equal(expectedUnits.Count, actualUnits.Count);
    }
    [Fact]
    public void Save_ShouldWriteSerializedProductionUnitsToFile()
    {
        // Arrange
        var assetManager = new AssetManager();
        var expectedJson = JsonSerializer.Serialize(assetManager.productionUnits);

        // Use a temporary directory
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        try
        {
            // Act
            assetManager.Save(tempDirectory);

            // Assert
            string expectedPath = Path.Combine(tempDirectory, "ResultDataManager", "SavedJsonData.json");
            Assert.True(File.Exists(expectedPath), "File was not created.");

            string actualJson = File.ReadAllText(expectedPath);
            Assert.Equal(expectedJson, actualJson);
        }
        finally
        {
            // Clean up the temporary directory
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }
}



