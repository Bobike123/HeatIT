using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ScottPlot.Avalonia;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ScottPlot;
using System;
using Avalonia;


namespace SemesterProject.Views
{
    public partial class HeatProductionWindow : Window
    {
        public HeatProductionWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
        }

        public void DisplayHeatDemandContent(int[] dateColumns, int heatDemandColumn, string period)
        {
            AssetManager assetManager = new();
            assetManager.LoadChanged();
            AvaPlot myPlot = this.Find<AvaPlot>("AvaPlot1")!;
            myPlot.Plot.Clear();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv");

            string[][] Dates = SourceDataManager.CSVDisplayGraph(path, dateColumns);
            string[][] heatDemandData = SourceDataManager.CSVDisplayGraph(path, new int[] { heatDemandColumn });
            double[][] heatDemandDataDouble = Optimizer.ConvertToDoubleArray(heatDemandData);

            
            for (int x = 0; x < Dates.Length; x++)
            {
                double nextBarBase = 0;
                double heatDemandValue = heatDemandDataDouble[x][0];
                double gasBoilerHeat = double.Parse(assetManager.productionUnits[0].MaxHeat!);
                double oilBoilerHeat = double.Parse(assetManager.productionUnits[1].MaxHeat!);
                double gasMotorHeat = double.Parse(assetManager.productionUnits[2].MaxHeat!);
                double electricBoilerHeat = double.Parse(assetManager.productionUnits[3].MaxHeat!);

                // Create two bars if the value is greater than 3.6
                if (heatDemandValue > gasMotorHeat)
                {
                    // First bar up to 3.6 in blue
                    Bar gasMotorBar = new()
                    {
                        Size = 1,
                        Position = x,
                        ValueBase = nextBarBase,
                        FillColor = ScottPlot.Colors.Blue,
                        Value = nextBarBase + gasMotorHeat,
                        BorderColor = ScottPlot.Colors.Blue,
                    };
                    myPlot.Plot.Add.Bar(gasMotorBar);
                    nextBarBase += gasMotorHeat;

                    if (nextBarBase < heatDemandValue)
                    {

                        // Remaining part in orange
                        Bar gasBoilerBar = new()
                        {
                            Size = 1,
                            Position = x,
                            ValueBase = nextBarBase,
                            FillColor = ScottPlot.Colors.Orange,
                            Value = nextBarBase + (heatDemandValue - gasMotorHeat),
                            BorderColor = ScottPlot.Colors.Orange,
                        };
                        myPlot.Plot.Add.Bar(gasBoilerBar);
                        nextBarBase += gasBoilerHeat;

                        if (nextBarBase < heatDemandValue)
                        {
                            Bar oilBoilerBar = new()
                            {
                                Size = 1,
                                Position = x,
                                ValueBase = nextBarBase,
                                FillColor = ScottPlot.Colors.Red,
                                Value = nextBarBase + (heatDemandValue - gasBoilerHeat),
                                BorderColor = ScottPlot.Colors.Red,
                            };
                            nextBarBase += oilBoilerHeat;
                            myPlot.Plot.Add.Bar(oilBoilerBar);
                            if (nextBarBase < heatDemandValue)
                            {
                                Bar electricBoilerBar = new()
                                {
                                    Size = 1,
                                    Position = x,
                                    ValueBase = nextBarBase,
                                    FillColor = ScottPlot.Colors.Green,
                                    Value = nextBarBase + (heatDemandValue - oilBoilerHeat),
                                    BorderColor = ScottPlot.Colors.Green,
                                };
                                myPlot.Plot.Add.Bar(electricBoilerBar);
                                nextBarBase += electricBoilerHeat;
                            }
                        }
                    }
                }
                else
                {
                    // Single bar if value is less than or equal to 3.6
                    Bar bar = new()
                    {
                        Size = 1,
                        Position = x,
                        ValueBase = nextBarBase,
                        FillColor = ScottPlot.Colors.Blue,
                        Value = nextBarBase + heatDemandValue,
                        BorderColor = ScottPlot.Colors.Blue,
                    };

                    myPlot.Plot.Add.Bar(bar);
                    nextBarBase += heatDemandValue;
                }

            }

            // Use custom tick labels on the bottom
            ScottPlot.TickGenerators.NumericManual tickGen = new();
            tickGen.AddMajor(0, period == "summer" ? $"{Dates[0][0]}" + ".07" : $"{Dates[0][0]}" + ".02");
            for (int x = 1; x < Dates.Length; x++)
            {
                if (Dates[x][0] != Dates[x - 1][0])
                {

                    if (period == "summer")
                    {
                        tickGen.AddMajor(x, $"{Dates[x][0]}" + ".07");
                    }
                    else
                    {
                        tickGen.AddMajor(x, $"{Dates[x][0]}" + ".02");
                    };

                }
            }
            myPlot.Plot.Axes.Bottom.TickGenerator = tickGen;


            // Set axes limits and refresh the plot
            myPlot.Plot.Axes.Margins(bottom: 0, top: 0, left: 0, right: 0);
            myPlot.Plot.Axes.SetLimitsY(0, Optimizer.CalculateMax(new int[] { heatDemandColumn }, 1));
            myPlot.Refresh();

        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            WinterPeriod.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(211, 211, 211));
            SummerPeriod.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(207, 3, 3));
            DisplayHeatDemandContent(new int[] { 0, 1 }, 2, "winter");
        }

        public void SummerPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(211, 211, 211));
            WinterPeriod.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(207, 3, 3));
            DisplayHeatDemandContent(new int[] { 4, 5 }, 6, "summer");
        }
    }
}
