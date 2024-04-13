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
        public static void CSVDisplayGraph(string filePath, int[] columns)
        {
            
            // Read from the CSV file
            string[][] data = [];
            using (StreamReader reader = new StreamReader(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                data = new string[lines.Length-3][];
                for (int i = 3,j=0; i < lines.Length; i++,j++)
                {
                    data[j] = lines[i].Split(",");
                }
            }
            string[][] newdata = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                newdata[i] = new string[columns.Length];
                for (int j = 0; j < columns.Length; j++)
                {
                    newdata[i][j] = data[i][columns[j]];
                }
            }
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "newfile.csv");
            AppendToCSV(csvFilePath, newdata);
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
