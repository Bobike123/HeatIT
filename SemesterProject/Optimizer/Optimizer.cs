using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using SemesterProject.Views;

namespace SemesterProject;
public class Optimizer
{
    public static double[][] Calculation(string period, string[][] olas)
    {
        double[][] unitsSUMMER = new double[olas.Length][];
        double[][] unitsWINTER = new double[olas.Length][];

        // Initialize each inner array
        for (int i = 0; i < unitsSUMMER.Length; i++)
        {
            unitsSUMMER[i] = new double[4];
            unitsWINTER[i] = new double[4];

        }

        for (int i = 0; i < ReadFile.productionUnits.Count; i++)
        {
            for (int j = 0; j < olas.Length; ++j)
            {
                double v = ChangeToDouble(ReadFile.productionUnits[i].MaxElectricity!);
                double c = ChangeToDouble(ReadFile.productionUnits[i].ProductionCosts!);
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
        for(int i = 0; i< unitsSUMMER.Length; ++i)
        {
            Array.Sort(unitsSUMMER[i],  new Comparison<double>( 
            (x,y) => { return x < y ? -1 : (x > y ? 1 : 0); }
            ));
        }
        
        for(int i = 0; i< unitsWINTER.Length; ++i)
        {
            Array.Sort(unitsWINTER[i],  new Comparison<double>( 
            (x,y) => { return x < y ? -1 : (x > y ? 1 : 0); }
            ));
        }

        string[][] orderedSummer = Convert(unitsSUMMER);
        string[][] orderedWinter = Convert(unitsWINTER);
        // Do something with 'units' array, such as writing to a CSV file
        ResultDataManager.AppendToCSV("newFile.csv", orderedWinter);
        string[][] empty = {
            [""],
            [""]
        };
        ResultDataManager.AppendToCSV("newFile.csv", empty);

        ResultDataManager.AppendToCSV("newFile.csv", orderedSummer);

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
        for (int i = 0; i < sumWinter.Length; i++)
        {
            Console.WriteLine($"Winter for the {i} unit {sumWinter[i]:00.00}");
            Console.WriteLine($"Summer for the {i} unit {sumsummer[i]:00.00}");
        }
        if(period == "winter")
        {
            return unitsWINTER;
        }
        else
        {
            return unitsSUMMER;
        }
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
        for(int i = 0; i < values.Length; ++i)
        {
            stringArray[i] = new string[values[i].Length];
            for(int j = 0; j < values[i].Length; ++j)
            {
                stringArray[i][j] = values[i][j].ToString();
            }
        }
        return stringArray;
    }
    public static void OperatingPoint(string[][] data, double[][] cost, int period)
    {
        double[][] skies = new double[data.Length][];
        double[][] operatingPoint = new double[data.Length][];
        for(int i = 0; i < data.Length; ++i)
        {
            skies[i] =  new double[ReadFile.productionUnits.Count];
            operatingPoint[i] =  new double[ReadFile.productionUnits.Count];
            for(int j = 0; j < ReadFile.productionUnits.Count; ++j)
            {
                operatingPoint[i][j] = ChangeToDouble(data[i][period]) / ChangeToDouble(ReadFile.productionUnits[j].MaxHeat!);
            }
        }
        
        string[][] operatingPointString = Convert(operatingPoint);
        ResultDataManager.AppendToCSV("operatingPoint.csv", operatingPointString);
    }
}
