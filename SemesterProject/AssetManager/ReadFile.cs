using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SemesterProject
{
    public class ReadFile
    {
        public const string Path = "units.json";
        public static ProductionUnit[] productionUnits =
        [
            new ProductionUnit("GB", 5.0, 0, 500, 215, "gas"),
            new ProductionUnit("OB", 4.0, 0, 700, 265, "oil"),
            new ProductionUnit("GM", 3.6, 2.7, 1100, 640, "gas"),
            new ProductionUnit("EK", 8.0, -8.0, 50, 0, "electricity"),
        ];        
        public void Save()
        {
            string? json = JsonSerializer.Serialize(productionUnits);
            File.WriteAllText(Path, json);
        }
        public void Load()
        {
            if(Exist())
            {
                string? json = File.ReadAllText(Path);
                productionUnits = JsonSerializer.Deserialize<ProductionUnit[]>(json)!;
            }
        }
        public static bool Exist()
        {
            if (File.Exists(Path))
            {
                return true;
            }
            else
            {
                File.Create(Path);
                return true;
            }
        }
    }
}