using Avalonia;
using System.IO;
using Avalonia.Media;
using System.Text.Json;
using Avalonia.Logging;
using Avalonia.Controls;
using System.Diagnostics;
using System.Collections.Generic;
using Avalonia.Controls.ApplicationLifetimes;

namespace SemesterProject
{
    public class ReadFile
    {
        public static string PathJson = Path.Combine(Directory.GetCurrentDirectory(), "AssetManager", "units.json");
        public static ProductionUnit[] productionUnits =
        [
            new ProductionUnit("GB", "5.0", "0", "500", "215", "gas"),
            new ProductionUnit("OB", "4.0", "0", "700", "265", "oil"),
            new ProductionUnit("GM", "3.6", "2.7", "1100", "640", "gas"),
            new ProductionUnit("EK", "8.0", "-8.0", "50", "0", "electricity"),
        ];
        public void Save()
        {
            string? json = JsonSerializer.Serialize(productionUnits);
            File.WriteAllText(PathJson, json);
        }
        public void Load()
        {
            if(Exist())
            {
                string? json = File.ReadAllText(PathJson);
                productionUnits = JsonSerializer.Deserialize<ProductionUnit[]>(json)!;
            }
        }
        public static bool Exist()
        {
            if (File.Exists(PathJson))
            {
                return true;
            }
            else
            {
                File.Create(PathJson);
                return true;
            }
        }
    }
}