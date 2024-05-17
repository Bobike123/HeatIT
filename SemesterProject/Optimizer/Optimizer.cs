using System;
using System.IO;
using System.Collections.Generic;
using SemesterProject.Views;

namespace SemesterProject;
public class Optimizer
{
    public static double Calculation(string period, int boiler, int day)
    {
        List<ProductionUnit> productionUnits = AssetManager.productionUnits;

        string[][] olas = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), [2, 6]);
        double[][] unitsSUMMER = new double[olas.Length][];
        double[][] unitsWINTER = new double[olas.Length][];

        // Initialize each inner array
        for (int i = 0; i < unitsSUMMER.Length; i++)
        {
            unitsSUMMER[i] = new double[4];
            unitsWINTER[i] = new double[4];

        }

        for (int i = 0; i < productionUnits.Count; i++)
        {
            for (int j = 0; j < olas.Length; ++j)
            {
                double v = ChangeToDouble(productionUnits[i].MaxElectricity!);
                double c = ChangeToDouble(productionUnits[i].ProductionCosts!);
                if (v > 0)
                {
                    unitsSUMMER[j][i] = c - (v * ChangeToDouble(olas[j][1]));
                    unitsWINTER[j][i] = c - (v * ChangeToDouble(olas[j][0]));
                }
                else if (v < 0)
                {
                    unitsSUMMER[j][i] = c + (Math.Abs(v) * ChangeToDouble(olas[j][1]));
                    unitsWINTER[j][i] = c + (Math.Abs(v) * ChangeToDouble(olas[j][0]));
                }
                else
                {
                    unitsSUMMER[j][i] = c;
                    unitsWINTER[j][i] = c;
                }

            }
        }
        for (int i = 0; i < unitsSUMMER.Length; ++i)
        {
            Array.Sort(unitsSUMMER[i], new Comparison<double>(
            (x, y) => { return x < y ? -1 : (x > y ? 1 : 0); }
            ));
        }

        for (int i = 0; i < unitsWINTER.Length; ++i)
        {
            Array.Sort(unitsWINTER[i], new Comparison<double>(
            (x, y) => { return x < y ? -1 : (x > y ? 1 : 0); }
            ));
        }

        string[][] orderedSummer = Convert(unitsSUMMER);
        string[][] orderedWinter = Convert(unitsWINTER);
        // Do something with 'units' array, such as writing to a CSV file
        string[][] empty = [
            [""],
            [""]
        ];

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
            return unitsWINTER[boiler][day];
        }
        else
        {
            return unitsSUMMER[boiler][day];
        }
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
    public static double[] CalculateValue(int day, string max, string period)
    {
        //calculate what values should be put for each boiler in that specific day.
        //max is the heat demand that has to bea reached with the 4 boilers so that max profit can be achieve
        //unitsSUMMER[j][i] = c + (Math.Abs(v) * ChangeToDouble(olas[j][1]));
        //unitsWINTER[j][i] = c + (Math.Abs(v) * ChangeToDouble(olas[j][0]));
        double[] units = [AssetManager.productionUnits.Count];
        for (int boiler = 0; boiler < AssetManager.productionUnits.Count; boiler++)
        {
            units[boiler] = Calculation(period, boiler, day);
        }

        return [3.5, 2, 4];
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
