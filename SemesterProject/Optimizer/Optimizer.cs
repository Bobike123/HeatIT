using System;
using System.IO;
using System.Collections.Generic;
using SemesterProject.Views;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SemesterProject;
public class Optimizer
{
    public static double[][] Calculation(string period, string[][] data)
    {
        List<ProductionUnit> productionUnits = AssetManager.productionUnits;
        double[][] unitsSUMMER = new double[data.Length][];
        double[][] unitsWINTER = new double[data.Length][];

        // Initialize each inner array
        for (int i = 0; i < unitsSUMMER.Length; i++)
        {
            unitsSUMMER[i] = new double[productionUnits.Count];
            unitsWINTER[i] = new double[productionUnits.Count];

        }

        for (int i = 0; i < productionUnits.Count; i++)
        {
            for (int j = 0; j < data.Length; ++j)
            {
                double v = ChangeToDouble(productionUnits[i].MaxElectricity!);
                double c = ChangeToDouble(productionUnits[i].ProductionCosts!);
                if (v > 0)
                {
                    unitsSUMMER[j][i] = c - (v * ChangeToDouble(data[j][1]));
                    unitsWINTER[j][i] = c - (v * ChangeToDouble(data[j][0]));
                }
                else if (v < 0)
                {
                    unitsSUMMER[j][i] = c + (Math.Abs(v) * ChangeToDouble(data[j][1]));
                    unitsWINTER[j][i] = c + (Math.Abs(v) * ChangeToDouble(data[j][0]));
                }
                else
                {
                    unitsSUMMER[j][i] = c;
                    unitsWINTER[j][i] = c;
                }

            }
        }
        // Do something with 'units' array, such as writing to a CSV file

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
            ResultDataManager.AppendDoubleArrayToCSV(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "newfile.csv"), unitsWINTER);
            return unitsWINTER;
        }
        else
        {
            ResultDataManager.AppendDoubleArrayToCSV(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "newfile.csv"), unitsSUMMER);

            return unitsSUMMER;
        }
    }

    public static double[] CalculateValue(string max, int day, string period, string[][] data)
    {
        double[] units = new double[AssetManager.productionUnits.Count];
        string p = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "newfile.csv");
        double maxHeat = double.Parse(max);
        double[][] calculatedUnits = Calculation(period, data);

        // Initialize all units to 0
        for (int unit = 0; unit < units.Length; unit++)
        {
            units[unit] = 0;
        }

        // Get sorted positions based on the heat production values of the units for the given day
        int[] sortedarray = GetSortedPositions(calculatedUnits[day]);
        double remainingHeat = maxHeat;

        foreach (int position in sortedarray)
        {
            double maxHeatProduction = double.Parse(AssetManager.productionUnits[position].MaxHeat!);

            if (remainingHeat > maxHeatProduction)
            {
                units[position] = maxHeatProduction;
                remainingHeat -= maxHeatProduction;
            }
            else
            {
                units[position] = remainingHeat;
                remainingHeat = 0;
                break;
            }
        }

        ResultDataManager.AppendDoubleArrayToCSV(p, [units]);

        return units;
    }

    private static int[] GetSortedPositions(double[] units)
    {
        return units
            .Select((value, index) => new { Value = value, Index = index })
            .OrderBy(x => x.Value)
            .Select(x => x.Index)
            .ToArray();
    }

    public static double CalculateMax(int[] columns, double multiplier)
    {
        double max = -99999;
        string[][] calculateMax = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), columns);
        for (int i = 0; i < calculateMax.Length; i++)
        {
            for (int j = 0; j < columns.Length; j++)
                if (double.Parse(calculateMax[i][j]) * multiplier > max) max = double.Parse(calculateMax[i][j]) * multiplier;//calculates the maximum variable
        }
        return max * 1.2;
    }


    public static double ChangeToDouble(string value)
    {
        try
        {
            double doubleValue = double.Parse(value);
            return doubleValue;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot convert: {ex}");
            return 0;
        }
    }
    public static string[][] Convert(double[][] values)
    {
        string[][] stringArray = new string[values.Length][];
        for (int i = 0; i < values.Length; ++i)
        {
            stringArray[i] = new string[values[i].Length];
            for (int j = 0; j < values[i].Length; ++j)
            {
                stringArray[i][j] = values[i][j].ToString();
            }
        }
        return stringArray;
    }
    public static double[][] OperatingPoint(string[][] data, List<ProductionUnit> productionUnits, int period)
    {
        double[][] skies = new double[data.Length][];
        double[][] operatingPoint = new double[data.Length][];
        for (int i = 0; i < data.Length; ++i)
        {
            skies[i] = new double[productionUnits.Count];
            operatingPoint[i] = new double[productionUnits.Count];
            for (int j = 0; j < productionUnits.Count; ++j)
            {
                operatingPoint[i][j] = ChangeToDouble(data[i][period]) / ChangeToDouble(productionUnits[j].MaxHeat!);
            }
        }

        return operatingPoint;
    }
}
