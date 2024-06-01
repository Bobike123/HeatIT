using Avalonia;
using System.IO;
using Avalonia.Media;
using Avalonia.Controls;
using ScottPlot.Avalonia;
using Avalonia.Interactivity;
using System.Linq;
using System;

namespace SemesterProject.Views
{

    public partial class CO2EmissionWindow : Window
    {

        public CO2EmissionWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
        }
        public void DisplayGraphContent(int periodInt, string period)
        {
            AssetManager assetManager = new();
            assetManager.LoadChanged();
            AvaPlot avaPlot1 = this.Find<AvaPlot>("AvaPlot1")!;//initializes the graph
            avaPlot1.Plot.Clear();

            string[][] dates = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), [0, 4]);

            double[][] datesDouble = Optimizer.ConvertToDoubleArray(dates);
            string[][] heatDemand = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), [2, 6]);
            double[][] heatDemandDouble = Optimizer.ConvertToDoubleArray(heatDemand);
            double[] operatingPoint = new double[heatDemandDouble.Length];
            double[] data_X = new double[datesDouble.Length];
            double[] data_Y_GasBoiler = new double[dates.Length];
            double[] data_Y_OilBoiler = new double[dates.Length];
            double[] data_Y_GasMotor = new double[dates.Length];
            double[] data_Y_ElectricBoiler = new double[dates.Length];
            double max = double.MinValue, min = double.MaxValue, sum = 0;
            double count = 0;
            double gasBoilerCo2 = double.Parse(assetManager.productionUnits[0].CO2Emissions!);
            double oilBoilerCo2 = double.Parse(assetManager.productionUnits[1].CO2Emissions!);
            double gasMotorCo2 = double.Parse(assetManager.productionUnits[2].CO2Emissions!);
            double electricBoilerCo2 = double.Parse(assetManager.productionUnits[3].CO2Emissions!);
            double gasBoilerHeat = double.Parse(assetManager.productionUnits[0].MaxHeat!);
            double oilBoilerHeat = double.Parse(assetManager.productionUnits[1].MaxHeat!);
            double gasMotorHeat = double.Parse(assetManager.productionUnits[2].MaxHeat!);
            double electricBoilerHeat = double.Parse(assetManager.productionUnits[3].MaxHeat!);
            double totalExpense;
            double heatDemandValue;
            int unit = 0;
            int nonZeroCount = 0;


            for (int x = 0; x < heatDemandDouble.Length; x++)
            {
                heatDemandValue = heatDemandDouble[x][periodInt];
                operatingPoint[x] = 0;
                data_X[x] = datesDouble[x][periodInt] + count * 0.041; //creates the x axis
                count = (count + 1) % 24;

                if (heatDemandValue > gasMotorHeat)
                {
                    // GasMotor
                    heatDemandValue -= gasMotorHeat;
                    if (heatDemandValue < gasBoilerHeat)
                    {
                        // GasBoiler
                        operatingPoint[x] = 1;
                        unit = 0;
                        data_Y_GasMotor[x] = gasMotorCo2 * operatingPoint[x];
                        heatDemandDouble[x][periodInt] -= gasMotorHeat;
                        operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                        data_Y_GasBoiler[x] = gasBoilerCo2 * operatingPoint[x];
                    }
                    else
                    {
                        heatDemandValue -= gasBoilerHeat;
                        operatingPoint[x] = 0;
                        if (heatDemandValue < oilBoilerHeat)
                        {
                            //OilBoiler
                            operatingPoint[x] = 1;
                            unit = 1;
                            data_Y_GasMotor[x] = gasBoilerCo2 * operatingPoint[x];
                            data_Y_GasBoiler[x] = gasBoilerCo2 * operatingPoint[x];
                            heatDemandDouble[x][periodInt] -= gasBoilerHeat;
                            heatDemandDouble[x][periodInt] -= gasMotorHeat;
                            operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                            data_Y_OilBoiler[x] = oilBoilerCo2 * operatingPoint[x];
                        }
                        else
                        {
                            heatDemandValue -= oilBoilerHeat;
                            operatingPoint[x] = 0;
                            if (heatDemandValue < electricBoilerHeat)
                            {
                                //ElectricBoiler
                                operatingPoint[x] = 1;
                                unit = 3;
                                data_Y_GasMotor[x] = gasMotorCo2 * operatingPoint[x];
                                data_Y_GasBoiler[x] = gasBoilerCo2 * operatingPoint[x];
                                data_Y_OilBoiler[x] = oilBoilerCo2 * operatingPoint[x];
                                heatDemandDouble[x][periodInt] -= gasBoilerHeat;
                                heatDemandDouble[x][periodInt] -= gasMotorHeat;
                                heatDemandDouble[x][periodInt] -= oilBoilerHeat;
                                operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                                data_Y_ElectricBoiler[x] = electricBoilerCo2 * operatingPoint[x];
                            }
                            else
                            {
                                heatDemandValue -= electricBoilerCo2;
                            }
                        }
                    }
                }
                else
                {
                    unit = 2;
                    operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                    data_Y_GasMotor[x] = gasMotorCo2 * operatingPoint[x];
                }
                
                totalExpense = data_Y_GasBoiler[x] + data_Y_OilBoiler[x] + data_Y_GasMotor[x] + data_Y_ElectricBoiler[x];
                if (totalExpense > max) max = totalExpense;
                if (totalExpense < min) min = totalExpense;
                
                if (totalExpense > 0)
                {
                    sum += totalExpense;
                    nonZeroCount++;
                }
            }
            double average =  sum / nonZeroCount;

            highestTextBlock.Text = $"{max:00.00} KG/MWh";
            lowestTextBlock.Text = $"{min:00.00} KG/MWh";
            averageTextBlock.Text = $"{average:00.00} KG/MWh";

            if (period == "summer")
                avaPlot1.Plot.Title($"CO2 Emissions Graph for Summer Period", size: 20);
            else
                avaPlot1.Plot.Title($"CO2 Emissions Graph for Winter Period", size: 20);

            avaPlot1.Plot.XLabel("Days", size: 15);
            avaPlot1.Plot.YLabel("KG/MWh", size: 15);
            var plotGB = AvaPlot1.Plot.Add.Scatter(data_X, data_Y_GasBoiler, ScottPlot.Colors.Orange);
            var plotOB = AvaPlot1.Plot.Add.Scatter(data_X, data_Y_OilBoiler, ScottPlot.Colors.Red);
            var plotGM = AvaPlot1.Plot.Add.Scatter(data_X, data_Y_GasMotor, ScottPlot.Colors.Blue);
            var plotEB = AvaPlot1.Plot.Add.Scatter(data_X, data_Y_ElectricBoiler, ScottPlot.Colors.Green);
            plotGB.MarkerSize = 0;
            plotOB.MarkerSize = 0;
            plotGM.MarkerSize = 0;
            plotEB.MarkerSize = 0;
            avaPlot1.Plot.Axes.SetLimitsX(8, 14.95);
            avaPlot1.Plot.Axes.SetLimitsY(0, max * 1.2);
            
            //Adds graph labels to x-axis
            ScottPlot.TickGenerators.NumericManual tickGen = new();
            
            for (int x = 0; x < heatDemandDouble.Length; x++)
            {
                

                if (x % 24 == 0) // Add major ticks at each day start
                {
                    string label = (period == "summer") ? $"{data_X[x]}.07" : $"{data_X[x]}.02";
                    tickGen.AddMajor(data_X[x], label);
                }
                if(x % 24 == 12)
                {
                    string hourLabel = $"12:00";
                    tickGen.AddMajor(data_X[x],hourLabel);
                }
                tickGen.AddMinor(data_X[x]);
                
                
            }
            avaPlot1.Plot.Axes.Bottom.TickGenerator = tickGen;
            
            avaPlot1.Refresh();
        }

        public void SummerPeriodButton(object sender, RoutedEventArgs args)
        {
            WinterPeriod.Background = new SolidColorBrush(Colors.Gray);
            SummerPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayGraphContent(1, "summer");
        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Colors.Gray);
            WinterPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayGraphContent(0, "winter");
        }
    }
}
