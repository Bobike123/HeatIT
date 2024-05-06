using System;

namespace SemesterProject;
public class Optimizer
{
    public static void Calculation(string[][] olas)
    {
        //double[][] netProduction = new double[olas.Length][];
        //double[] array = new double[ReadFile.productionUnits.Length];
        string[][] unitsSUMMER = new string[olas.Length][];
        string[][] unitsWINTER = new string[olas.Length][];

        // Initialize each inner array
        for (int i = 0; i < unitsSUMMER.Length; i++)
        {
            unitsSUMMER[i] = new string[4];
            unitsWINTER[i] = new string[4];

        }

        for (int i = 0; i < ReadFile.productionUnits.Length; i++)
        {
            for (int j = 0; j < olas.Length; ++j)
            {
                double v = ChangeToDouble(ReadFile.productionUnits[i].MaxElectricity!);
                double c = ChangeToDouble(ReadFile.productionUnits[i].ProductionCosts!);
                if (v > 0)
                {
                    unitsSUMMER[j][i] = (c - (v * ChangeToDouble(olas[j][1]))).ToString("00.00");
                    unitsWINTER[j][i] = (c - (v * ChangeToDouble(olas[j][0]))).ToString("00.00");
                }
                else if (v < 0)
                {
                    unitsSUMMER[j][i] = (c + (Math.Abs(v) * ChangeToDouble(olas[j][1]))).ToString("00.00");
                    unitsWINTER[j][i] = (c + (Math.Abs(v) * ChangeToDouble(olas[j][0]))).ToString("00.00");
                }
                else
                {
                    unitsSUMMER[j][i] = c.ToString();
                    unitsWINTER[j][i] = c.ToString();
                }

            }
        }

        // Do something with 'units' array, such as writing to a CSV file
        ResultDataManager.AppendToCSV("newFile.csv", unitsWINTER);
        string[][] empty = {
            [""],
            [""]
        };
        ResultDataManager.AppendToCSV("newFile.csv", empty);

        ResultDataManager.AppendToCSV("newFile.csv", unitsSUMMER);

        double[] sumWinter = new double[unitsWINTER[0].Length];
        double[] sumsummer = new double[unitsSUMMER[0].Length];

        for (int i = 0; i < unitsWINTER.Length; i++)
        {
            for (int j = 0; j < unitsWINTER[i].Length; j++)
            {
                sumWinter[j] += ChangeToDouble(unitsWINTER[i][j]);
                sumsummer[j] += ChangeToDouble(unitsSUMMER[i][j]);

            }
        }
        for (int i = 0; i < sumWinter.Length; i++)
        {
            Console.WriteLine($"Winter for the {i} unit {sumWinter[i]:00.00}");
            Console.WriteLine($"Summer for the {i} unit {sumsummer[i]:00.00}");
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
}
