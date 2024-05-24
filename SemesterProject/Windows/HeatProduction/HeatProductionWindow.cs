using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ScottPlot.Avalonia;
using System.IO;
using System.Collections.Generic;
using ScottPlot.Colormaps;
using System;
using Microsoft.CodeAnalysis;
using ScottPlot;
using System.Linq;

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
            AssetManager assetManager = new();
            AvaPlot myPlot = this.Find<AvaPlot>("AvaPlot1")!;
            myPlot.Plot.Clear();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv");
            string[][] Dates;
            string[][] heatDemand;
            ScottPlot.Color[] categoryColors = { ScottPlot.Colors.Orange, ScottPlot.Colors.Red, ScottPlot.Colors.Blue, ScottPlot.Colors.Green };

            if (period == "summer")
            {
                Dates = SourceDataManager.CSVDisplayGraph(path, new int[] { 0, 1, 3 }); // Adjust columns for summer period
                heatDemand = SourceDataManager.CSVDisplayGraph(path, new int[] { 2 }); // Adjust columns for summer period
            }
            else
            {
                Dates = SourceDataManager.CSVDisplayGraph(path, new int[] { 4, 5, 3 }); // Adjust columns for winter period
                heatDemand = SourceDataManager.CSVDisplayGraph(path, new int[] { 6 }); // Adjust columns for winter period
            }

            for (int x = 0; x < Dates.Length; x++)
            {
                double nextBarBase = 0;
                double[] values = Optimizer.CalculateValue(heatDemand[x][0], x, period, Dates, assetManager);

                // Get the current electricity price for the selected period
                double electricityPrice = double.Parse(Dates[x][2]); // Assuming electricity price is at column index 2 in the data

                // Sort production units by production costs
                var sortedProductionUnits = assetManager.productionUnits.OrderBy(unit => double.Parse(unit.ProductionCosts!));

                foreach (var productionUnit in sortedProductionUnits)
                {
                    double maxElectricity = double.Parse(productionUnit.MaxElectricity!);
                    double productionCost = double.Parse(productionUnit.ProductionCosts!);

                    // Adjust production cost based on electricity price
                    if (maxElectricity > 0)
                    {
                        productionCost += electricityPrice * maxElectricity;
                    }

                    Bar bar = new()
                    {
                        Size = 1,
                        Position = x,
                        ValueBase = nextBarBase,
                        FillColor = categoryColors[assetManager.productionUnits.IndexOf(productionUnit)],
                        Value = nextBarBase + values[assetManager.productionUnits.IndexOf(productionUnit)],
                        BorderColor = categoryColors[assetManager.productionUnits.IndexOf(productionUnit)],
                    };

                    myPlot.Plot.Add.Bar(bar);
                    nextBarBase += values[assetManager.productionUnits.IndexOf(productionUnit)]; // Update the base for the next bar
                }
            }

            // Use custom tick labels on the bottom
            ScottPlot.TickGenerators.NumericManual tickGen = new();
            tickGen.AddMajor(0, $"{Dates[0][0]}");
            for (int x = 1; x < Dates.Length; x++)
            {
                if (Dates[x][0] != Dates[x - 1][0])
                {
                    tickGen.AddMajor(x, $"{Dates[x][0]}");
                }
            }

            // Set axes limits and refresh the plot
            myPlot.Plot.Axes.Margins(bottom: 0, top: 0);
            myPlot.Plot.Axes.SetLimitsY(0, Optimizer.CalculateMax(new int[] { 2, 6 }, 1));
            myPlot.Refresh();
        }

        /*
                    string[][] newData = SourceDataManager.CSVDisplayGraph(path, columns);
                    //newData is siplified data that can be used for the graph
                    double[] data_X = new double[newData.Length];
                    double[] data_Y = new double[newData.Length];
                    double[] yAxis = new double[newData.Length];
                    double[] boilBoiler = new double[newData.Length];
                    int count = 0;//cout for the 24 hours of the day used for the x axis
                    for (int i = 0; i < newData.Length; i++)
                    {
                        data_X[i] = double.Parse(newData[i][0]) + count * 0.041;//creates the x axis
                        data_Y[i] = double.Parse(newData[i][1]);                //creates the y axis
                        count = (count + 1) % 24;
                        yAxis[i] = 0;
                        boilBoiler[i] = double.Parse(ReadFile.productionUnits[0].MaxHeat!);
                    }
                    //modifies the highest lowest and average data from the axaml file

                    avaPlot1.Plot.XLabel("Days");
                    avaPlot1.Plot.YLabel("MWh");
                    avaPlot1.Plot.Add.Scatter(data_X, data_Y);
                    avaPlot1.Plot.Axes.SetLimits(8, 14.95);//one week
                    avaPlot1.Plot.Axes.SetLimitsY(0, 9);//comparation of the two periods

                    //modifies the title of the graph depending on the time of the year
                    if (period == "summer")
                    {
                        avaPlot1.Plot.Title("Heat Demand Graph for Summer Period");
                        avaPlot1.Plot.Add.FillY(data_X, yAxis, data_Y).FillStyle.Color = ScottPlot.Color.FromHex("#FFA500");
                    }
                    else
                    {
                        avaPlot1.Plot.Title("Heat Demand Graph for Winter Period");
                        avaPlot1.Plot.Add.FillY(data_X, boilBoiler, data_Y).FillStyle.Color = ScottPlot.Color.FromHex("#ff0000");
                        avaPlot1.Plot.Add.FillY(data_X, yAxis, boilBoiler).FillStyle.Color = ScottPlot.Color.FromHex("#FFA500");
                    }

                    avaPlot1.Refresh();
                    */
        public void SummerPeriodButton(object sender, RoutedEventArgs args)
        {
            WinterPeriod.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(211, 211, 211));
            SummerPeriod.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(207, 3, 3));
            DisplayHeatDemandContent([0, 2], "winter");//4 date 6 heat demand
        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(211, 211, 211));
            WinterPeriod.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(207, 3, 3));
            DisplayHeatDemandContent([4, 6], "summer");//0 date 2 heat demand
        }
    }
}