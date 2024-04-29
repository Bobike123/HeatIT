using Avalonia;
using System.IO;
using Avalonia.Media;
using Avalonia.Controls;
using ScottPlot.Avalonia;
using Avalonia.Interactivity;

namespace SemesterProject.Views
{

    public partial class CO2EmmisionWindow : Window
    {
        public CO2EmmisionWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
        }
        public void SummerPeriodButton(object sender, RoutedEventArgs args)
        {
            WinterPeriod.Background = new SolidColorBrush(Colors.Gray);
            SummerPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            //DisplayHeatDemandContent([4,6],"summer");//4 date 6 heat demand
        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Colors.Gray);
            WinterPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            //DisplayHeatDemandContent([0, 2],"winter");//0 date 2 heat demand
        }

        public void GasBoilerButton(object sender, RoutedEventArgs args)
        {
            GasBoiler.Background = new SolidColorBrush(Color.FromRgb(255, 165, 0));
            OilBoiler.Background = new SolidColorBrush(Colors.Gray);
            GasMotor.Background = new SolidColorBrush(Colors.Gray);
            //DisplayHeatDemandContent([0, 2],"winter");//0 date 2 heat demand
        }
        public void OilBoilerButton(object sender, RoutedEventArgs args)
        {
            OilBoiler.Background = new SolidColorBrush(Color.FromRgb(255, 165, 0));
            GasBoiler.Background = new SolidColorBrush(Colors.Gray);
            GasMotor.Background = new SolidColorBrush(Colors.Gray);
            //DisplayHeatDemandContent([0, 2],"winter");//0 date 2 heat demand
        }
        public void GasMotorButton(object sender, RoutedEventArgs args)
        {
            GasMotor.Background = new SolidColorBrush(Color.FromRgb(255, 165, 0));
            OilBoiler.Background = new SolidColorBrush(Colors.Gray);
            GasBoiler.Background = new SolidColorBrush(Colors.Gray);
            //DisplayHeatDemandContent([0, 2],"winter");//0 date 2 heat demand
        }
    }
}