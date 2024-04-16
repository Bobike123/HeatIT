using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Logging;
using System.Diagnostics;
using Avalonia.Media;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;

namespace SemesterProject.Views
{
    public class SourceDataManager
    {
        public static string[][] CSVDisplayGraph(string filePath, int[] columns)
        {
            // Read from the CSV file
            string[][] data;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                data = new string[lines.Length - 3][];
                for (int i = 3, j = 0; i < lines.Length; i++, j++)
                {
                    data[j] = lines[i].Split(",");
                }
            }

            string[][] newData = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                newData[i] = new string[columns.Length];
                for (int j = 0; j < columns.Length; j++)
                {
                    if (j == 0 || j == 1 || j == 4 || j == 5)
                    {
                        string[] splitValues = data[i][columns[j]].Split(new string[] { "/", "," }, StringSplitOptions.None);
                        if (splitValues.Length >= 2)
                        {
                            newData[i][j] = splitValues[1];
                        }
                    }
                    else
                    {
                        newData[i][j] = data[i][columns[j]];
                    }
                }
            }

            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "newfile.csv");
            AppendToCSV(csvFilePath, newData);
            return newData;
        }


        public static void AppendToCSV(string filePath, string[][] data)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                foreach (string[] line in data)
                {
                    writer.WriteLine(string.Join(",", line));
                }
            }
        }
    }
}
