using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace SemesterProject
{
    public class ProductionUnit
    {
        public string? Name { get; set; } //GB OB GM or EK
        public string? MaxHeat { get; set; } //MWh
        public string? MaxElectricity { get; set; } //MWh
        public string? ProductionCosts { get; set; } //DKK/MWh(th)
        public string? CO2Emissions { get; set; } //kg/MWh(th)
        public string? FuelType { get; set; } //gas oil or electricity

        public ProductionUnit(string name, string maxHeat, string maxElectricity, string productionCosts, string co2Emissions, string fuelType)
        {
            Name = name;
            MaxHeat = maxHeat;
            MaxElectricity = maxElectricity;
            ProductionCosts = productionCosts;
            CO2Emissions = co2Emissions;
            FuelType = fuelType;
        }
    }

    public class AssetManager
    {
        public static string PathJson = Path.Combine(Directory.GetCurrentDirectory(), "AssetManager", "units.json");
        public List<ProductionUnit> productionUnits = new List<ProductionUnit>
        {
            new ProductionUnit("GB", "5.0", "0", "500", "215", "gas"),
            new ProductionUnit("OB", "4.0", "0", "700", "265", "oil"),
            new ProductionUnit("GM", "3.6", "2.7", "1100", "640", "gas"),
            new ProductionUnit("EK", "8.0", "-8.0", "50", "0", "electricity"),
        };
        public void Save()
        {
            string? json = JsonSerializer.Serialize(productionUnits);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "ResultDataManager", "SavedJsonData.json"), json);
        }
        public void Load()
        {
            if (Exist())
            {
                string? json = File.ReadAllText(PathJson);
                productionUnits = JsonSerializer.Deserialize<List<ProductionUnit>>(json)!;
            }
        }
        public void LoadChanged()
        {
            if (Exist())
            {
                string? json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "ResultDataManager", "SavedJsonData.json"));
                productionUnits = JsonSerializer.Deserialize<List<ProductionUnit>>(json)!;
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