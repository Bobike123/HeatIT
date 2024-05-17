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
        private int _selection = -1; // Backing field to store the value of selection

        public int selection
        {
            get => _selection; // Getter returns the value of the backing field
            set => _selection = value; // Setter sets the value of the backing field
        }
        public CO2EmmisionWindow()
        {
            InitializeComponent();
            FirstUnit.Content = AssetManager.productionUnits[0].Name;
            SecondUnit.Content = AssetManager.productionUnits[1].Name;
            ThirdUnit.Content = AssetManager.productionUnits[2].Name;
            ForthUnit.Content = AssetManager.productionUnits[3].Name;
            this.AttachDevTools();
        }
        public void DisplayGraphContent(int[] columns, string period, int unit)
        {
            AvaPlot avaPlot1 = this.Find<AvaPlot>("AvaPlot1")!;//initializes the graph
            avaPlot1.Plot.Clear();
            //clears the graph if any previous information was displayed on it
            string[][] newData = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), columns);
            //newData is siplified data that can be used for the graph
            double max = -1;
            double[] data_X = new double[newData.Length], data_Y = new double[newData.Length];
            int count = 0;//cout for the 24 hours of the day used for the x axis
            for (int i = 0; i < newData.Length; i++)
            {
                data_X[i] = double.Parse(newData[i][0]) + count * 0.041;//creates the x axis
                data_Y[i] = double.Parse(newData[i][1]) * double.Parse(AssetManager.productionUnits[unit].CO2Emissions!);                //creates the y axis
                if (data_Y[i] > max) max = data_Y[i] + 100;
                count = (count + 1) % 24;
            }
            //modifies the highest lowest and average data from the axaml file

            //modifies the title of the graph depending on the time of the year
            if (period == "summer") avaPlot1.Plot.Title($"CO2 Emmisions for {AssetManager.productionUnits[unit].Name!} Graph for Summer Period");
            else avaPlot1.Plot.Title($"CO2 Emmisions for {AssetManager.productionUnits[unit].Name!} Graph for Winter Period");
            avaPlot1.Plot.XLabel("Days");
            avaPlot1.Plot.YLabel("KG/MWh");
            avaPlot1.Plot.Add.Scatter(data_X, data_Y);
            avaPlot1.Plot.Axes.SetLimitsX(8, 14.95);//one week
            avaPlot1.Plot.Axes.SetLimitsY(0, max);//comparation of the two periods
            avaPlot1.Refresh();
        }

        public void SummerPeriodButton(object sender, RoutedEventArgs args)
        {
            WinterPeriod.Background = new SolidColorBrush(Colors.Gray);
            SummerPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            if (selection != -1)
            {
                DisplayGraphContent([4, 6], "summer", selection);
            }
            //DisplayHeatDemandContent([4,6],"summer");//4 date 6 heat demand
        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Colors.Gray);
            WinterPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            if (selection != -1)
            {
                DisplayGraphContent([0, 2], "winter", selection);
            }
            //DisplayHeatDemandContent([0, 2],"winter");//0 date 2 heat demand
        }

        public void FirstUnitButton(object sender, RoutedEventArgs args)
        {
            selection = 0;
            FirstUnit.Background = new SolidColorBrush(Color.FromRgb(255, 165, 0));
            SecondUnit.Background = new SolidColorBrush(Colors.Gray);
            ThirdUnit.Background = new SolidColorBrush(Colors.Gray);
            ForthUnit.Background = new SolidColorBrush(Colors.Gray);
            if (SummerPeriod.Background!.Equals(Color.FromRgb(207, 3, 3))) SummerPeriodButton(sender, args);
            else if (WinterPeriod.Background!.Equals(Color.FromRgb(207, 3, 3))) WinterPeriodButton(sender, args);
            //DisplayHeatDemandContent([0, 2],"winter");//0 date 2 heat demand
        }
        public void SecondUnitButton(object sender, RoutedEventArgs args)
        {
            selection = 1;
            FirstUnit.Background = new SolidColorBrush(Colors.Gray);
            SecondUnit.Background = new SolidColorBrush(Color.FromRgb(255, 165, 0));
            ThirdUnit.Background = new SolidColorBrush(Colors.Gray);
            ForthUnit.Background = new SolidColorBrush(Colors.Gray);
            if (SummerPeriod.Background!.Equals(Color.FromRgb(207, 3, 3))) SummerPeriodButton(sender, args);
            else if (WinterPeriod.Background!.Equals(Color.FromRgb(207, 3, 3))) WinterPeriodButton(sender, args);
            //DisplayHeatDemandContent([0, 2],"winter");//0 date 2 heat demand
        }
        public void ThirdUnitButton(object sender, RoutedEventArgs args)
        {
            selection = 2;
            FirstUnit.Background = new SolidColorBrush(Colors.Gray);
            SecondUnit.Background = new SolidColorBrush(Colors.Gray);
            ThirdUnit.Background = new SolidColorBrush(Color.FromRgb(255, 165, 0));
            ForthUnit.Background = new SolidColorBrush(Colors.Gray);
            if (SummerPeriod.Background!.Equals(Color.FromRgb(207, 3, 3))) SummerPeriodButton(sender, args);
            else if (WinterPeriod.Background!.Equals(Color.FromRgb(207, 3, 3))) WinterPeriodButton(sender, args);
            //DisplayHeatDemandContent([0, 2],"winter");//0 date 2 heat demand
        }
        public void ForthUnitButton(object sender, RoutedEventArgs args)
        {
            selection = 3;
            FirstUnit.Background = new SolidColorBrush(Colors.Gray);
            SecondUnit.Background = new SolidColorBrush(Colors.Gray);
            ThirdUnit.Background = new SolidColorBrush(Colors.Gray);
            ForthUnit.Background = new SolidColorBrush(Color.FromRgb(255, 165, 0));
            if (SummerPeriod.Background!.Equals(Color.FromRgb(207, 3, 3))) SummerPeriodButton(sender, args);
            else if (WinterPeriod.Background!.Equals(Color.FromRgb(207, 3, 3))) WinterPeriodButton(sender, args);
            //DisplayHeatDemandContent([0, 2],"winter");//0 date 2 heat demand
        }
    }
}