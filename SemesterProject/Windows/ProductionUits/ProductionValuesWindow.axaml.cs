using Avalonia;
using SemesterProject;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.Diagnostics.Telemetry;
using ScottPlot.Plottables;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SemesterProject.Views
{

    public partial class ProductionUnitsWindow : Window
    {
        public ProductionUnitsWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
            Load();
        }
        public void Load()
        {
            AName.Text=ReadFile.productionUnits[0].Name;
            AMH.Text=ReadFile.productionUnits[0].MaxHeat;
            AME.Text=ReadFile.productionUnits[0].MaxElectricity;
            APC.Text=ReadFile.productionUnits[0].ProductionCosts;
            ACO2.Text=ReadFile.productionUnits[0].CO2Emissions;
            AFT.Text=ReadFile.productionUnits[0].FuelType;

            BName.Text=ReadFile.productionUnits[1].Name;
            BMH.Text=ReadFile.productionUnits[1].MaxHeat;
            BME.Text=ReadFile.productionUnits[1].MaxElectricity;
            BPC.Text=ReadFile.productionUnits[1].ProductionCosts;
            BCO2.Text=ReadFile.productionUnits[1].CO2Emissions;
            BFT.Text=ReadFile.productionUnits[1].FuelType;

            CName.Text=ReadFile.productionUnits[2].Name;
            CMH.Text=ReadFile.productionUnits[2].MaxHeat;
            CME.Text=ReadFile.productionUnits[2].MaxElectricity;
            CPC.Text=ReadFile.productionUnits[2].ProductionCosts;
            CCO2.Text=ReadFile.productionUnits[2].CO2Emissions;
            CFT.Text=ReadFile.productionUnits[2].FuelType;

            DName.Text=ReadFile.productionUnits[3].Name;
            DMH.Text=ReadFile.productionUnits[3].MaxHeat;
            DME.Text=ReadFile.productionUnits[3].MaxElectricity;
            DPC.Text=ReadFile.productionUnits[3].ProductionCosts;
            DCO2.Text=ReadFile.productionUnits[3].CO2Emissions;
            DFT.Text=ReadFile.productionUnits[3].FuelType;
        }
        public void GetTableContent(object sender, RoutedEventArgs args)
        {
            ReadFile readFile = new();
            //Make so that you can access the data
            ReadFile.productionUnits[0].Name=AName.Text;
            ReadFile.productionUnits[0].MaxHeat=AMH.Text;
            ReadFile.productionUnits[0].MaxElectricity=AME.Text;
            ReadFile.productionUnits[0].ProductionCosts=APC.Text;
            ReadFile.productionUnits[0].CO2Emissions=ACO2.Text;
            

            ReadFile.productionUnits[1].Name=BName.Text;
            ReadFile.productionUnits[1].MaxHeat=BMH.Text;
            ReadFile.productionUnits[1].MaxElectricity=BME.Text;
            ReadFile.productionUnits[1].ProductionCosts=BPC.Text;
            ReadFile.productionUnits[1].CO2Emissions=BCO2.Text;
            ReadFile.productionUnits[1].FuelType=BFT.Text;

            ReadFile.productionUnits[2].Name=CName.Text;
            ReadFile.productionUnits[2].MaxHeat=CMH.Text;
            ReadFile.productionUnits[2].MaxElectricity=CME.Text;
            ReadFile.productionUnits[2].ProductionCosts=CPC.Text;
            ReadFile.productionUnits[2].CO2Emissions=CCO2.Text;
            ReadFile.productionUnits[2].FuelType=CFT.Text;

            ReadFile.productionUnits[3].Name=DName.Text;
            ReadFile.productionUnits[3].MaxHeat=DMH.Text;
            ReadFile.productionUnits[3].MaxElectricity=DME.Text;
            ReadFile.productionUnits[3].ProductionCosts=DPC.Text;
            ReadFile.productionUnits[3].CO2Emissions=DCO2.Text;
            ReadFile.productionUnits[3].FuelType=DFT.Text;
            readFile.Save();
        }
    }
}