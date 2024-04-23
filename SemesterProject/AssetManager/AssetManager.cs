using System;
using System.Data;

namespace SemesterProject
{
    public class ProductionUnit
    {
        public string? Name {get; set;} //GB OB GM or EK
        public string? MaxHeat {get; set;} //MW
        public string? MaxElectricity {get; set;} //MW 
        public string? ProductionCosts {get; set;} //DKK/MWh(th)
        public string? CO2Emissions {get; set;} //kg/MWh(th)
        public string? FuelType {get; set;} //gas oil or electricity

        public ProductionUnit(string name, string maxHeat, string maxElectricity, string productionCosts, string co2Emissions, string fuelType)
        {
            Name = name;
            MaxHeat = maxHeat;
            MaxElectricity = maxElectricity;
            ProductionCosts = productionCosts;
            CO2Emissions = co2Emissions;
            FuelType = fuelType;
        }
        public static ProductionUnit GetProductionUnit(string name)
        {
            foreach (var unit in ReadFile.productionUnits)
            {
                if (unit.Name == name)
                return unit;
            }
            return new ProductionUnit("", "", "", "", "", "");
        }
    }

}