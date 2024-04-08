using System;
using System.Data;
using SemesterProject;
public class AssetManager
{
    public static ProductionUnit[] ProductionUnits { get; internal set; }

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

    public static ProductionUnit GetProductionUnit(string name)
    {
        foreach (var unit in ReadFile.productionUnits)
        {
            if (unit.Name == name)
            return unit;
        }
        return new ProductionUnit("", 0, 0, 0, 0, "");
    }
}