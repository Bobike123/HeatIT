using System;
using System.IO;
using SemesterProject.Views;

namespace SemesterProject;
public class Optimizer
{
    public static double[][] Calculation(string period, string[][] data, AssetManager assetManager)
    {

        double[][] unitsSUMMER = new double[data.Length][];
        double[][] unitsWINTER = new double[data.Length][];

        // Initialize each inner array
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
            //ResultDataManager.AppendDoubleArrayToCSV(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "newfile.csv"), unitsWINTER);
            return unitsWINTER;
        }
        else
        {
            //ResultDataManager.AppendDoubleArrayToCSV(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "newfile.csv"), unitsSUMMER);
            return unitsSUMMER;
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
    public static double[][] ConvertToDoubleArray(string[][] values)
    {
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

    public static double[] OperatingPoint(double[][] data, AssetManager assetManager, int period, int unit)
    {
        double[][] skies = new double[data.Length][];
        double[] operatingPoint = new double[data.Length];
        for (int i = 0; i < data.Length; ++i)
        {
            skies[i] = new double[assetManager.productionUnits.Count];
            operatingPoint[i] = data[i][period] / ChangeToDouble(assetManager.productionUnits[unit].MaxHeat!);

        }

        return operatingPoint;
    }
}
