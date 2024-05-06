using System;
using System.IO;

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
                for (int i = 3, j = 0; i < lines.Length; i++, j++)//deletes the first 3 rows and stores the rest of the file in to data
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
                    if (columns[j] == 0 || columns[j] == 1 || columns[j] == 4 || columns[j] == 5)//check to see if the columns are dates or not
                    {
                        string[] splitValues = data[i][columns[j]].Split(new string[] { "/", "," }, StringSplitOptions.None);//only takes the day from the date
                        if (splitValues.Length >= 2)
                        {
                            newData[i][j] = splitValues[1];
                        }
                    }
                    else // not dates, no modify
                    {
                        newData[i][j] = data[i][columns[j]];
                    }
                }
            }
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "newfile.csv");
            ResultDataManager.AppendToCSV(csvFilePath, newData);
            return newData;
        }
    }
}
