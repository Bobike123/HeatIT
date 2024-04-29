using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ScottPlot.Avalonia;
using System.IO;
using ScottPlot.Colormaps;
using System;
using Microsoft.CodeAnalysis;

namespace SemesterProject.Views
{

    public partial class HeatProductionWindow : Window
    {
        public HeatProductionWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
        }

        public void DisplayHeatDemandContent(int[] columns, string period)
        {
            AvaPlot avaPlot1 = this.Find<AvaPlot>("AvaPlot1")!;//initializes the graph
            avaPlot1.Plot.Clear();
            //clears the graph if any previous information was displayed on it
            string[][] newData = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), columns);
            //newData is siplified data that can be used for the graph
            double min = 200, max = -1, av;
            double[] data_X = new double[newData.Length];
            double[] data_Y = new double[newData.Length]; 
            double[] yAxis = new double[newData.Length];
            double[] boilBoiler = new double[newData.Length];
            int count = 0;//cout for the 24 hours of the day used for the x axis
            for (int i = 0; i < newData.Length; i++)
            {
                data_X[i] = double.Parse(newData[i][0]) + count * 0.041;//creates the x axis
                data_Y[i] = double.Parse(newData[i][1]);                //creates the y axis
                if (double.Parse(newData[i][1]) > max) max = double.Parse(newData[i][1]);//calculates the maximum variable
                if (double.Parse(newData[i][1]) < min) min = double.Parse(newData[i][1]);//calculates the minimum variable
                count = (count + 1) % 24;
                yAxis[i] = 0;
                boilBoiler[i] = double.Parse(ReadFile.productionUnits[0].MaxHeat!);
            }
            //modifies the highest lowest and average data from the axaml file
            av = (max + min) / 2;
            highest.Text = max.ToString("0.00");
            lowest.Text = min.ToString("0.00");
            average.Text = av.ToString("0.00");

            avaPlot1.Plot.XLabel("Days");
            avaPlot1.Plot.YLabel("MWh");
            avaPlot1.Plot.Add.Scatter(data_X, data_Y);
            avaPlot1.Plot.Axes.SetLimits(8, 14.95);//one week
            avaPlot1.Plot.Axes.SetLimitsY(0, 9);//comparation of the two periods

            //modifies the title of the graph depending on the time of the year
            if (period == "summer") 
            {
                avaPlot1.Plot.Title("Heat Demand Graph for Summer Period");
                avaPlot1.Plot.Add.FillY(data_X, yAxis, data_Y);
            }
            else 
            {
                avaPlot1.Plot.Title("Heat Demand Graph for Winter Period");
                avaPlot1.Plot.Add.FillY(data_X, yAxis, boilBoiler);
                avaPlot1.Plot.Add.FillY(data_X, boilBoiler, data_Y);
            }

            avaPlot1.Refresh();
        }
        public void SummerPeriodButton(object sender, RoutedEventArgs args)
        {
            WinterPeriod.Background = new SolidColorBrush(Colors.Gray);
            SummerPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayHeatDemandContent([4, 6], "summer");//4 date 6 heat demand
        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Colors.Gray);
            WinterPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayHeatDemandContent([0, 2], "winter");//0 date 2 heat demand
        }
    }
}