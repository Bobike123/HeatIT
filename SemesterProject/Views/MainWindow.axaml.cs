using Avalonia;
using SemesterProject;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SemesterProject.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AssetManager assetManager = new AssetManager();
            assetManager.Save();
            InitializeComponent();
            this.AttachDevTools();
        }

        public void ModifyProfitExpensesIncome(object sender, RoutedEventArgs args)
        {
            income.Text = "";
            expenses.Text = "";
            profit.Text = "";
        }
        public void OpenCO2Emissions(object sender, RoutedEventArgs args)
        {
            var cO2EmissionsWindow = new CO2EmissionWindow();
            cO2EmissionsWindow.Show();
        }
        public void OpenExpensesAndProfit(object sender, RoutedEventArgs args)
        {
            var expensesAndProfitWindow = new ExpensesAndProfitWindow();
            expensesAndProfitWindow.Show();
        }
        public void OpenHeatDemand(object sender, RoutedEventArgs args)
        {
            var heatDemandWindow = new HeatDemandWindow();
            heatDemandWindow.Show();
        }

        public void OpenHeatProduction(object sender, RoutedEventArgs args)
        {
            var heatProductionWindow = new HeatProductionWindow();
            heatProductionWindow.Show();
        }

        public void OpenProductionUits(object sender, RoutedEventArgs args)
        {
            var productionUitsWindow = new ProductionUnitsWindow();
            productionUitsWindow.Show();
        }
        public void OpenProductionValues(object sender, RoutedEventArgs args)
        {
            var productionValuesWindow = new ProductionValuesWindow();
            productionValuesWindow.Show();
        }
    }
}
