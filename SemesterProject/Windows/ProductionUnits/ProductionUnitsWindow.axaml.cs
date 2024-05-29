using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

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
            AssetManager assetManager = new();
            assetManager.LoadChanged();
            AName.Text = assetManager.productionUnits[0].Name;
            AMH.Text = assetManager.productionUnits[0].MaxHeat;
            AME.Text = assetManager.productionUnits[0].MaxElectricity;
            APC.Text = assetManager.productionUnits[0].ProductionCosts;
            ACO2.Text = assetManager.productionUnits[0].CO2Emissions;
            AFT.Text = assetManager.productionUnits[0].FuelType;

            BName.Text = assetManager.productionUnits[1].Name;
            BMH.Text = assetManager.productionUnits[1].MaxHeat;
            BME.Text = assetManager.productionUnits[1].MaxElectricity;
            BPC.Text = assetManager.productionUnits[1].ProductionCosts;
            BCO2.Text = assetManager.productionUnits[1].CO2Emissions;
            BFT.Text = assetManager.productionUnits[1].FuelType;

            CName.Text = assetManager.productionUnits[2].Name;
            CMH.Text = assetManager.productionUnits[2].MaxHeat;
            CME.Text = assetManager.productionUnits[2].MaxElectricity;
            CPC.Text = assetManager.productionUnits[2].ProductionCosts;
            CCO2.Text = assetManager.productionUnits[2].CO2Emissions;
            CFT.Text = assetManager.productionUnits[2].FuelType;

            DName.Text = assetManager.productionUnits[3].Name;
            DMH.Text = assetManager.productionUnits[3].MaxHeat;
            DME.Text = assetManager.productionUnits[3].MaxElectricity;
            DPC.Text = assetManager.productionUnits[3].ProductionCosts;
            DCO2.Text = assetManager.productionUnits[3].CO2Emissions;
            DFT.Text = assetManager.productionUnits[3].FuelType;
        }
        public void SaveTableContent(object sender, RoutedEventArgs args)
        {
            AssetManager assetManager = new();
            //Make so that you can access the data
            assetManager.productionUnits[0].Name = AName.Text;
            assetManager.productionUnits[0].MaxHeat = AMH.Text;
            assetManager.productionUnits[0].MaxElectricity = AME.Text;
            assetManager.productionUnits[0].ProductionCosts = APC.Text;
            assetManager.productionUnits[0].CO2Emissions = ACO2.Text;


            assetManager.productionUnits[1].Name = BName.Text;
            assetManager.productionUnits[1].MaxHeat = BMH.Text;
            assetManager.productionUnits[1].MaxElectricity = BME.Text;
            assetManager.productionUnits[1].ProductionCosts = BPC.Text;
            assetManager.productionUnits[1].CO2Emissions = BCO2.Text;
            assetManager.productionUnits[1].FuelType = BFT.Text;

            assetManager.productionUnits[2].Name = CName.Text;
            assetManager.productionUnits[2].MaxHeat = CMH.Text;
            assetManager.productionUnits[2].MaxElectricity = CME.Text;
            assetManager.productionUnits[2].ProductionCosts = CPC.Text;
            assetManager.productionUnits[2].CO2Emissions = CCO2.Text;
            assetManager.productionUnits[2].FuelType = CFT.Text;

            assetManager.productionUnits[3].Name = DName.Text;
            assetManager.productionUnits[3].MaxHeat = DMH.Text;
            assetManager.productionUnits[3].MaxElectricity = DME.Text;
            assetManager.productionUnits[3].ProductionCosts = DPC.Text;
            assetManager.productionUnits[3].CO2Emissions = DCO2.Text;
            assetManager.productionUnits[3].FuelType = DFT.Text;
            assetManager.Save();
            assetManager.LoadChanged();
        }
    }
}