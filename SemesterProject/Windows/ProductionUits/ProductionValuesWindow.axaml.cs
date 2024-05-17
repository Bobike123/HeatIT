using Avalonia;
using SemesterProject;
using Avalonia.Controls;
using System.IO;
using System.Text.Json;
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
            AName.Text = AssetManager.productionUnits[0].Name;
            AMH.Text = AssetManager.productionUnits[0].MaxHeat;
            AME.Text = AssetManager.productionUnits[0].MaxElectricity;
            APC.Text = AssetManager.productionUnits[0].ProductionCosts;
            ACO2.Text = AssetManager.productionUnits[0].CO2Emissions;
            AFT.Text = AssetManager.productionUnits[0].FuelType;

            BName.Text = AssetManager.productionUnits[1].Name;
            BMH.Text = AssetManager.productionUnits[1].MaxHeat;
            BME.Text = AssetManager.productionUnits[1].MaxElectricity;
            BPC.Text = AssetManager.productionUnits[1].ProductionCosts;
            BCO2.Text = AssetManager.productionUnits[1].CO2Emissions;
            BFT.Text = AssetManager.productionUnits[1].FuelType;

            CName.Text = AssetManager.productionUnits[2].Name;
            CMH.Text = AssetManager.productionUnits[2].MaxHeat;
            CME.Text = AssetManager.productionUnits[2].MaxElectricity;
            CPC.Text = AssetManager.productionUnits[2].ProductionCosts;
            CCO2.Text = AssetManager.productionUnits[2].CO2Emissions;
            CFT.Text = AssetManager.productionUnits[2].FuelType;

            DName.Text = AssetManager.productionUnits[3].Name;
            DMH.Text = AssetManager.productionUnits[3].MaxHeat;
            DME.Text = AssetManager.productionUnits[3].MaxElectricity;
            DPC.Text = AssetManager.productionUnits[3].ProductionCosts;
            DCO2.Text = AssetManager.productionUnits[3].CO2Emissions;
            DFT.Text = AssetManager.productionUnits[3].FuelType;
        }
        public void SaveTableContent(object sender, RoutedEventArgs args)
        {
            AssetManager readFile = new();
            //Make so that you can access the data
            AssetManager.productionUnits[0].Name = AName.Text;
            AssetManager.productionUnits[0].MaxHeat = AMH.Text;
            AssetManager.productionUnits[0].MaxElectricity = AME.Text;
            AssetManager.productionUnits[0].ProductionCosts = APC.Text;
            AssetManager.productionUnits[0].CO2Emissions = ACO2.Text;


            AssetManager.productionUnits[1].Name = BName.Text;
            AssetManager.productionUnits[1].MaxHeat = BMH.Text;
            AssetManager.productionUnits[1].MaxElectricity = BME.Text;
            AssetManager.productionUnits[1].ProductionCosts = BPC.Text;
            AssetManager.productionUnits[1].CO2Emissions = BCO2.Text;
            AssetManager.productionUnits[1].FuelType = BFT.Text;

            AssetManager.productionUnits[2].Name = CName.Text;
            AssetManager.productionUnits[2].MaxHeat = CMH.Text;
            AssetManager.productionUnits[2].MaxElectricity = CME.Text;
            AssetManager.productionUnits[2].ProductionCosts = CPC.Text;
            AssetManager.productionUnits[2].CO2Emissions = CCO2.Text;
            AssetManager.productionUnits[2].FuelType = CFT.Text;

            AssetManager.productionUnits[3].Name = DName.Text;
            AssetManager.productionUnits[3].MaxHeat = DMH.Text;
            AssetManager.productionUnits[3].MaxElectricity = DME.Text;
            AssetManager.productionUnits[3].ProductionCosts = DPC.Text;
            AssetManager.productionUnits[3].CO2Emissions = DCO2.Text;
            AssetManager.productionUnits[3].FuelType = DFT.Text;
            readFile.Save();
            //readFile.Load();
        }
    }
}