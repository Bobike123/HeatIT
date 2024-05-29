using Xunit;
using System.Collections.Generic;
using System.Linq;
using System;
using SemesterProject;

public class CalculationTests
{
    public class ProductionUnit
    {
        public string? MaxElectricity { get; set; }
        public string? ProductionCosts { get; set; }
    }

    public class AssetManager
    {
        public List<ProductionUnit> productionUnits { get; set; } = new List<ProductionUnit>();
    }

    private static double ChangeToDouble(string str)
    {
        return double.TryParse(str, out double result) ? result : 0;
    }

    [Fact]
    public void Calculation_WinterPeriod_ReturnsCorrectUnitsWinter()
    {
        // Arrange
        string period = "winter";
        string[][] data = new string[][]
        {
            new string[] { "1.0", "2.0" },
            new string[] { "3.0", "4.0" }
        };
        var assetManager = new AssetManager();
        assetManager.productionUnits.Add(new ProductionUnit { MaxElectricity = "10", ProductionCosts = "100" });
        assetManager.productionUnits.Add(new ProductionUnit { MaxElectricity = "-5", ProductionCosts = "200" });

        // Act
        double[][] result = Calculation(period, data, assetManager);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Equal(2, result[0].Length);
        Assert.Equal(2, result[1].Length);

        Assert.Equal(90.0, result[0][0]); // 100 - (10 * 1.0)
        Assert.Equal(205.0, result[0][1]); // 200 + (5 * 1.0)
        Assert.Equal(70.0, result[1][0]); // 100 - (10 * 3.0)
        Assert.Equal(215.0, result[1][1]); // 200 + (5 * 3.0)
    }

    [Fact]
    public void Calculation_SummerPeriod_ReturnsCorrectUnitsSummer()
    {
        // Arrange
        string period = "summer";
        string[][] data = new string[][]
        {
            new string[] { "1.0", "2.0" },
            new string[] { "3.0", "4.0" }
        };
        var assetManager = new AssetManager();
        assetManager.productionUnits.Add(new ProductionUnit { MaxElectricity = "10", ProductionCosts = "100" });
        assetManager.productionUnits.Add(new ProductionUnit { MaxElectricity = "-5", ProductionCosts = "200" });

        // Act
        double[][] result = Calculation(period, data, assetManager);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Equal(2, result[0].Length);
        Assert.Equal(2, result[1].Length);

        Assert.Equal(80.0, result[0][0]); // 100 - (10 * 2.0)
        Assert.Equal(210.0, result[0][1]); // 200 + (5 * 2.0)
        Assert.Equal(60.0, result[1][0]); // 100 - (10 * 4.0)
        Assert.Equal(220.0, result[1][1]); // 200 + (5 * 4.0)
    }

    //Calculation method from Optimizer.Calculation needs to be included in this test class
    public static double[][] Calculation(string period, string[][] data, AssetManager assetManager)
    {
        double[][] unitsSUMMER = new double[data.Length][];
        double[][] unitsWINTER = new double[data.Length][];

        for (int i = 0; i < unitsSUMMER.Length; i++)
        {
            unitsSUMMER[i] = new double[assetManager.productionUnits.Count];
            unitsWINTER[i] = new double[assetManager.productionUnits.Count];
        }

        for (int i = 0; i < assetManager.productionUnits.Count; i++)
        {
            for (int j = 0; j < data.Length; ++j)
            {
                double e = ChangeToDouble(assetManager.productionUnits[i].MaxElectricity!);
                double c = ChangeToDouble(assetManager.productionUnits[i].ProductionCosts!);
                if (e > 0)
                {
                    unitsSUMMER[j][i] = c - (e * ChangeToDouble(data[j][1]));
                    unitsWINTER[j][i] = c - (e * ChangeToDouble(data[j][0]));
                }
                else if (e < 0)
                {
                    unitsSUMMER[j][i] = c + (Math.Abs(e) * ChangeToDouble(data[j][1]));
                    unitsWINTER[j][i] = c + (Math.Abs(e) * ChangeToDouble(data[j][0]));
                }
                else
                {
                    unitsSUMMER[j][i] = c;
                    unitsWINTER[j][i] = c;
                }
            }
        }

        double[] sumWinter = new double[unitsWINTER[0].Length];
        double[] sumsummer = new double[unitsSUMMER[0].Length];

        for (int i = 0; i < unitsWINTER.Length; i++)
        {
            for (int j = 0; j < unitsWINTER[i].Length; j++)
            {
                sumWinter[j] += unitsWINTER[i][j];
                sumsummer[j] += unitsSUMMER[i][j];
            }
        }

        if (period == "winter")
        {
            return unitsWINTER;
        }
        else
        {
            return unitsSUMMER;
        }
    }
    [Fact]
    public void Optimizer_ChangeToDouble()
    {
        //Arrange
        double[][] doubleValue = new double[][]{
                [2.1,3.2,1.2],
                [3.1,4.5,6.7]
            };
        //Act
        string[][] expected = new string[][]{
                ["2.1","3.2","1.2"],
                ["3.1","4.5","6.7"]
            };
        //Assert
        Assert.Equal(expected, SemesterProject.Optimizer.Convert(doubleValue));
    }

    public class StringArrayConverter
    {
        public static double[][] ConvertToDoubleArray(string[][] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            double[][] stringArray = new double[values.Length][];
            for (int i = 0; i < values.Length; ++i)
            {
                stringArray[i] = new double[values[i].Length];
                for (int j = 0; j < values[i].Length; ++j)
                {
                    stringArray[i][j] = double.Parse(values[i][j]);
                }
            }
            return stringArray;
        }
    }

    public class ConverterTests
    {
        [Fact]
        public void ConvertToDoubleArray_ValidInput_ReturnsCorrectResult()
        {
            // Arrange
            string[][] validInput = new string[][]
            {
            new string[] {"1", "2", "3"},
            new string[] {"4.5", "6.7", "8.9"}
            };

            // Act
            var result = StringArrayConverter.ConvertToDoubleArray(validInput);

            // Assert
            Assert.Equal(1, result[0][0]);
            Assert.Equal(2, result[0][1]);
            Assert.Equal(3, result[0][2]);
            Assert.Equal(4.5, result[1][0]);
            Assert.Equal(6.7, result[1][1]);
            Assert.Equal(8.9, result[1][2]);
        }

        [Fact]
        public void ConvertToDoubleArray_InvalidInput_ThrowsException()
        {
            // Arrange
            string?[][] invalidInput = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => StringArrayConverter.ConvertToDoubleArray(invalidInput!));
        }

        [Fact]
        public void ConvertToDoubleArray_InvalidStringInput_ThrowsException()
        {
            // Arrange
            string[][] invalidStringInput = new string[][]
            {
            new string[] {"1", "2", "three"},
            new string[] {"4.5", "6.7", "eight"}
            };

            // Act & Assert
            Assert.Throws<FormatException>(() => StringArrayConverter.ConvertToDoubleArray(invalidStringInput));
        }
    }
    [Fact]
    public void OperatingPoint_Returns_Correct_Values()
    {
        // Arrange
        double[][] testData = new double[][]
        {
            new double[] { 100, 200, 300 },  // Sample data row 1
            new double[] { 150, 250, 350 },  // Sample data row 2
            // Add more sample data rows as needed
        };

        // Use fully qualified name for AssetManager if it's in a different namespace
        SemesterProject.AssetManager assetManager = new SemesterProject.AssetManager();

        int period = 0; // Example period
        int unit = 0;   // Example unit

        // Act
        double[] result = Optimizer.OperatingPoint(testData, assetManager, period, unit);

        // Assert
        Assert.Equal(20, result[0]);
        Assert.Equal(30, result[1]);
    }
}



