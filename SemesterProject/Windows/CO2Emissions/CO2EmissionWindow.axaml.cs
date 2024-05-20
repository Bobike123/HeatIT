using Avalonia;
using System.IO;
using Avalonia.Media;
using Avalonia.Controls;
using ScottPlot.Avalonia;
using Avalonia.Interactivity;

namespace SemesterProject.Views
{

    public partial class CO2EmissionWindow : Window
    {
        private int _selection = -1; // Backing field to store the value of selection

        public int selection
        {
            get => _selection; // Getter returns the value of the backing field
            set => _selection = value; // Setter sets the value of the backing field
        }
        private TextBlock highestTextBlock;
        private TextBlock lowestTextBlock;
        private TextBlock averageTextBlock;


        public CO2EmissionWindow()
        {
            InitializeComponent();
            FirstUnit.Content = AssetManager.productionUnits[0].Name;
            SecondUnit.Content = AssetManager.productionUnits[1].Name;
            ThirdUnit.Content = AssetManager.productionUnits[2].Name;
            ForthUnit.Content = AssetManager.productionUnits[3].Name;
            this.AttachDevTools();
            
            highestTextBlock = highest;
            lowestTextBlock = lowest;
            averageTextBlock = average;

        }
        public void DisplayGraphContent(int[] columns, string period, int unit)
        {
            AvaPlot avaPlot1 = this.Find<AvaPlot>("AvaPlot1")!;//initializes the graph
            avaPlot1.Plot.Clear();
            //clears the graph if any previous information was displayed on it
            string[][] newData = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), columns);
            //newData is siplified data that can be used for the graph
            double[] data_X = new double[newData.Length], data_Y = new double[newData.Length];
            int count = 0;//cout for the 24 hours of the day used for the x axis

            double max = double.MinValue;
            double min = double.MaxValue;
            double sum = 0;

            for (int i = 0; i < newData.Length; i++)
            {
                data_X[i] = double.Parse(newData[i][0]) + count * 0.041;//creates the x axis
                data_Y[i] = double.Parse(newData[i][1]) * double.Parse(AssetManager.productionUnits[unit].CO2Emissions!);      
                count = (count + 1) % 24;

                if (data_Y[i] > max) max = data_Y[i];
                if (data_Y[i] < min) min = data_Y[i];
                sum += data_Y[i];
            }

            double average = sum / newData.Length;

            highestTextBlock.Text = $" {max.ToString("00.00")} KG/MWh";
            lowestTextBlock.Text = $" {min.ToString("00.00")} KG/MWh";
            averageTextBlock.Text = $" {average.ToString("00.00")} KG/MWh";
            

            for (int num = 0; num < AssetManager.productionUnits.Count; num++)
            if (double.Parse(AssetManager.productionUnits[num].CO2Emissions!) > max) max = double.Parse(AssetManager.productionUnits[num].CO2Emissions!);



            //modifies the title of the graph depending on the time of the year
            if (period == "summer") avaPlot1.Plot.Title($"CO2 Emissions for {AssetManager.productionUnits[unit].Name!} Graph for Summer Period", size: 20);
            else avaPlot1.Plot.Title($"CO2 Emissions for {AssetManager.productionUnits[unit].Name!} Graph for Winter Period", size: 20);
            avaPlot1.Plot.XLabel("Days", size: 15);
            avaPlot1.Plot.YLabel("KG/MWh", size: 15);
            avaPlot1.Plot.Add.Scatter(data_X, data_Y);
            avaPlot1.Plot.Axes.SetLimitsX(8, 14.95);//one week
            avaPlot1.Plot.Axes.SetLimitsY(0, Optimizer.CalculateMax([2, 6], max));//comparation of the two periods
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
            
        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Colors.Gray);
            WinterPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            if (selection != -1)
            {
                DisplayGraphContent([0, 2], "winter", selection);
            }
            
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
            
        }
    }
}