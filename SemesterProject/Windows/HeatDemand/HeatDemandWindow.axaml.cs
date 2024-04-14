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

namespace SemesterProject.Views
{

    public partial class HeatDemandWindow : Window
    {
        public HeatDemandWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
        }
        public void DisplayCSVContent(int[] columns)
        {
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager\\data.csv");
            string[][] newData = SourceDataManager.CSVDisplayGraph(csvFilePath, columns);
            double[] data_X = new double[newData.Length];
            double[] data_Y = new double[newData.Length];
            int count = 0;
            for (int i = 0; i < newData.Length; i++)
            {
                data_X[i] = double.Parse(newData[i][0]) + count * 0.041;
                data_Y[i] = double.Parse(newData[i][2]);
                count = (count + 1) % 24;

            }

            AvaPlot avaPlot1 = this.Find<AvaPlot>("AvaPlot1")!;
            avaPlot1.Plot.Add.Scatter(data_X, data_Y);
            avaPlot1.Refresh();
        }

        public void SummerPeriodButton(object sender, RoutedEventArgs args)
        {
            WinterPeriod.Background = new SolidColorBrush(Colors.Gray);
            SummerPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayCSVContent([4, 5, 6, 7]);
        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Colors.Gray);
            WinterPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayCSVContent([0, 1, 2, 3]);
        }
    }
}