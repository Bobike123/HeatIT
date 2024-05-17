using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SemesterProject;
public class ProductionUnitTests
{
    public ProductionUnitTests()
    {
        // Initialize the static list with some test data
        ReadFile.productionUnits = new List<ProductionUnit>
        {
            new ProductionUnit("Unit1", "100", "200", "300", "400", "Gas"),
            new ProductionUnit("Unit2", "150", "250", "350", "450", "Oil")
        };
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var name = "TestUnit";
        var maxHeat = "500";
        var maxElectricity = "600";
        var productionCosts = "700";
        var co2Emissions = "800";
        var fuelType = "Coal";

        // Act
        var unit = new ProductionUnit(name, maxHeat, maxElectricity, productionCosts, co2Emissions, fuelType);

        // Assert
        Assert.Equal(name, unit.Name);
        Assert.Equal(maxHeat, unit.MaxHeat);
        Assert.Equal(maxElectricity, unit.MaxElectricity);
        Assert.Equal(productionCosts, unit.ProductionCosts);
        Assert.Equal(co2Emissions, unit.CO2Emissions);
        Assert.Equal(fuelType, unit.FuelType);
    }

    [Fact]
    public void GetProductionUnit_ShouldReturnCorrectUnit()
    {
        // Act
        var unit = ProductionUnit.GetProductionUnit("Unit1");

        // Assert
        Assert.NotNull(unit);
        Assert.Equal("Unit1", unit.Name);
        Assert.Equal("100", unit.MaxHeat);
        Assert.Equal("200", unit.MaxElectricity);
        Assert.Equal("300", unit.ProductionCosts);
        Assert.Equal("400", unit.CO2Emissions);
        Assert.Equal("Gas", unit.FuelType);
    }

    [Fact]
    public void GetProductionUnit_ShouldReturnNewUnit_WhenNotFound()
    {
        // Act
        var unit = ProductionUnit.GetProductionUnit("NonExistentUnit");

        // Assert
        Assert.NotNull(unit);
        Assert.Equal("", unit.Name);
        Assert.Equal("", unit.MaxHeat);
        Assert.Equal("", unit.MaxElectricity);
        Assert.Equal("", unit.ProductionCosts);
        Assert.Equal("", unit.CO2Emissions);
        Assert.Equal("", unit.FuelType);
    }
}
