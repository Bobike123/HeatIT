using System;
using System.Data;
public class AssetManager
{
    public class ProductionUnit
    {
        public string Name {get; set;} //GB OB GM or EK
        public double MaxHeat {get; set;} //MW
        public double MaxElectricity {get; set;} //MW 
        public double ProductionCosts {get; set;} //DKK/MWh(th)
        public double CO2Emissions {get; set;} //kg/MWh(th)
        public string FuelType {get; set;} //gas oil or electricity

        public ProductionUnit(string name, double maxHeat, double maxElectricity, double productionCosts, double co2Emissions, string fuelType)
        {
            Name = name;
            MaxHeat = maxHeat;
            MaxElectricity = maxElectricity;
            ProductionCosts = productionCosts;
            CO2Emissions = co2Emissions;
            FuelType = fuelType;
        }
    }

    public static ProductionUnit[] ProductionUnits = new ProductionUnit[]
    {
        new ProductionUnit("GB", 5.0, 0, 500, 215, "gas"),
        new ProductionUnit("OB", 4.0, 0, 700, 265, "oil"),
        new ProductionUnit("GM", 3.6, 2.7, 1100, 640, "gas"),
        new ProductionUnit("EK", 8.0, -8.0, 50, 0, "electricity"),
    };

    public static ProductionUnit GetProductionUnit(string name)
    {
        foreach (var unit in ProductionUnits)
        {
            if (unit.Name == name)
            return unit;
        }
        return null;
    }
}