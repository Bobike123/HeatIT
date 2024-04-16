using System;
using Avalonia;
using System.IO;
using Avalonia.Media;
using Avalonia.Logging;
using Avalonia.Controls;
using System.Diagnostics;
using ScottPlot.Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Diagnostics;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using ScottPlot.Colormaps;

namespace SemesterProject.Views
{

    public partial class HeatDemandWindow : Window
    {
        public HeatDemandWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
        }
        public void DisplayCSVContent(int[] columns, string period)
        {
            AvaPlot avaPlot1 = this.Find<AvaPlot>("AvaPlot1")!;
            avaPlot1.Plot.Clear();
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv");
            string[][] newData = SourceDataManager.CSVDisplayGraph(csvFilePath, columns);
            double min=200,max=-1, av;
            double[] data_X = new double[newData.Length], data_Y = new double[newData.Length];
            int count = 0;
            for (int i = 0; i < newData.Length; i++)
            {
                data_X[i] = double.Parse(newData[i][0]) + count * 0.041;
                data_Y[i] = double.Parse(newData[i][2]);
                if(double.Parse(newData[i][2])>max)max=double.Parse(newData[i][2]);
                if(double.Parse(newData[i][2])<min)min=double.Parse(newData[i][2]);
                count = (count + 1) % 24;
            }
            av=(max+min)/2;
            highest.Text=max.ToString("0.00");
            lowest.Text=min.ToString("0.00");
            average.Text=av.ToString("0.00");
            
            if (period == "summer") avaPlot1.Plot.Title("Heat Demand Graph for Summer Period");
            else avaPlot1.Plot.Title("Heat Demand Graph for Winter Period");
            avaPlot1.Plot.XLabel("Days");
            avaPlot1.Plot.YLabel("MWh");
            avaPlot1.Plot.Add.Scatter(data_X, data_Y);
            avaPlot1.Plot.Axes.SetLimits(8,14.95);//one week
            avaPlot1.Plot.Axes.SetLimitsY(0,9);//comparation of the two periods
            avaPlot1.Refresh();
        }

        public void SummerPeriodButton(object sender, RoutedEventArgs args)
        {
            WinterPeriod.Background = new SolidColorBrush(Colors.Gray);
            SummerPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayCSVContent([4, 5, 6, 7],"summer");
        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Colors.Gray);
            WinterPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayCSVContent([0, 1, 2, 3],"winter");
        }
    }
}