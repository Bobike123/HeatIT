using System;
using System.IO;
using System.Linq;

namespace SemesterProject;
    public class Optimiser
    {
        public void Optimization()
        {
            var firstSc = ReadFile.productionUnits.OrderBy(unit => ChangeToDouble(unit.ProductionCosts!)).ToList();
            
            var secondSc = ReadFile.productionUnits.OrderBy(unit => 
            {
                double netProductionCost;
                if (ChangeToDouble(unit.MaxElectricity!) > 0)
                {
                    netProductionCost = ChangeToDouble(unit.ProductionCosts!) - (ChangeToDouble(unit.MaxElectricity!) * GetCurrentElectricityPrice());
                }
                else if(ChangeToDouble(unit.MaxElectricity!) < 0)
                {
                    netProductionCost = ChangeToDouble(unit.ProductionCosts!) + (Math.Abs(ChangeToDouble(unit.MaxElectricity!)) * GetCurrentElectricityPrice());
                }
                else
                {
                    netProductionCost = ChangeToDouble(unit.ProductionCosts!);
                }
                return netProductionCost;
            }).ToList();
        }
        private double GetCurrentElectricityPrice()
        {
            
            //string[][] newData = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), columns);

            
            //Implement logic to get current electricity price based on the time.
            return 100.00;
        }
        private static double ChangeToDouble(string value)
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