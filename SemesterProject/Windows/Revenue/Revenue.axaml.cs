using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using ScottPlot.Avalonia;

namespace SemesterProject.Views
{
    public partial class RevenueWindow : Window
    {
        private TextBlock ehighestTextBlock;
        private TextBlock elowestTextBlock;
        private TextBlock eaverageTextBlock;
        public RevenueWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
            ehighestTextBlock = ehighest;
            elowestTextBlock = elowest;
            eaverageTextBlock = eaverage;
        }
        
        public void DisplayExpensesContent(string period, int periodInt)
        {
            AssetManager assetManager = new();
            AvaPlot AvaPlot1 = this.Find<AvaPlot>("AvaPlot1")!;//initializes the graph
            assetManager.LoadChanged();
            AvaPlot1.Plot.Clear();
            //clears the graph if any previous information was displayed on it
            int unit = 0;
            string[][] newData = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), [3, 7]);
            string[][] heatDemand = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), [2, 6]);
            string[][] dates = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), [0, 4]);
            double[][] heatDemandDouble = Optimizer.ConvertToDoubleArray(heatDemand);
            double[] operatingPoint = new double[heatDemandDouble.Length];
            double[][] prices = [];

            double max = double.MinValue;
            double min = double.MaxValue;

            if (period == "summer")
                prices = Optimizer.Calculation("summer", newData, assetManager);
            else
                prices = Optimizer.Calculation("winter", newData, assetManager);
            //newData is simplified data that can be used for the graph
            double[] data_X = new double[newData.Length];
            double[] data_Y_GasBoiler = new double[newData.Length];
            double[] data_Y_OilBoiler = new double[newData.Length];
            double[] data_Y_GasMotor = new double[newData.Length];
            double[] data_Y_ElectricBoiler = new double[newData.Length];
            double gasBoilerHeat = double.Parse(assetManager.productionUnits[0].MaxHeat!);
            double oilBoilerHeat = double.Parse(assetManager.productionUnits[1].MaxHeat!);
            double gasMotorHeat = double.Parse(assetManager.productionUnits[2].MaxHeat!);
            double electricBoilerHeat = double.Parse(assetManager.productionUnits[3].MaxHeat!);
            double heatDemandValue;
            int count = 0;//count for the 24 hours of the day used for the x axis
            double totalExpense=0;
            double totalExpensePeriod=0;
            

            for (int x = 0; x < heatDemandDouble.Length; x++)
            {
                heatDemandValue = heatDemandDouble[x][periodInt];
                operatingPoint[x] = 0;
                data_X[x] = double.Parse(dates[x][0]) + count * 0.041;//creates the x axis
                count = (count + 1) % 24;

                // Create two bars if the value is greater than 3.6
                if (heatDemandValue > gasMotorHeat)
                {
                    // GasMotor
                    heatDemandValue -= gasMotorHeat;
                    if (heatDemandValue < gasBoilerHeat)
                    {
                        // GasBoiler
                        operatingPoint[x] = 1;
                        unit = 0;
                        data_Y_GasMotor[x] = prices[x][2] * operatingPoint[x]* (-1);
                        heatDemandDouble[x][periodInt] -= gasMotorHeat;
                        operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                        data_Y_GasBoiler[x] = prices[x][0] * operatingPoint[x] * (-1);
                    }
                    else
                    {
                        heatDemandValue -= gasBoilerHeat;
                        operatingPoint[x] = 0;
                        if(heatDemandValue < oilBoilerHeat)
                        {
                            //OilBoiler
                            operatingPoint[x] = 1;
                            unit = 1;
                            data_Y_GasMotor[x] = prices[x][2] * operatingPoint[x]* (-1);
                            data_Y_GasBoiler[x] = prices[x][0] * operatingPoint[x]* (-1);
                            heatDemandDouble[x][periodInt] -= gasBoilerHeat;
                            heatDemandDouble[x][periodInt] -= gasMotorHeat;
                            operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                            data_Y_OilBoiler[x] = prices[x][1] * operatingPoint[x]* (-1);
                        }
                        else
                        {
                            heatDemandValue -= oilBoilerHeat;
                            operatingPoint[x] = 0;
                            if(heatDemandValue < electricBoilerHeat)
                            {
                                //ElectricBoiler
                                operatingPoint[x] = 1;
                                unit = 3;
                                data_Y_GasMotor[x] = prices[x][2] * operatingPoint[x]* (-1);
                                data_Y_GasBoiler[x] = prices[x][0] * operatingPoint[x]* (-1);
                                data_Y_OilBoiler[x] = prices[x][1] * operatingPoint[x]* (-1);
                                heatDemandDouble[x][periodInt] -= gasBoilerHeat;
                                heatDemandDouble[x][periodInt] -= gasMotorHeat;
                                heatDemandDouble[x][periodInt] -= oilBoilerHeat;
                                operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                                data_Y_ElectricBoiler[x] = prices[x][3] * operatingPoint[x]* (-1);
                            }
                            else
                            {
                                heatDemandValue -= electricBoilerHeat;
                            }
                        }
                    }
                }else
                {
                    unit = 2;
                    operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                    data_Y_GasMotor[x] = prices[x][2] * operatingPoint[x]* (-1);
                }

                totalExpense = data_Y_GasBoiler[x] + data_Y_OilBoiler[x] + data_Y_GasMotor[x] + data_Y_ElectricBoiler[x];
                if (totalExpense > max) max = totalExpense;
                if (totalExpense < min) min = totalExpense;

                totalExpensePeriod +=totalExpense;

            }
            double av = totalExpensePeriod/newData.Length;
            ehighestTextBlock.Text = $"{max:00.00} DKK/MWh";
            elowestTextBlock.Text = $"{min:00.00} DKK/MWh";
            eaverageTextBlock.Text = $"{av:00.00} DKK/MWh";

            //modifies the title of the graph depending on the time of the year
            if (period == "summer") AvaPlot1.Plot.Title("Revenue Graph for Summer Period");
            else AvaPlot1.Plot.Title("Revenue Graph for Winter Period");
            AvaPlot1.Plot.XLabel("Days");
            AvaPlot1.Plot.YLabel("DKK/MWh");
            var plotGB = AvaPlot1.Plot.Add.Scatter(data_X, data_Y_GasBoiler, ScottPlot.Colors.Orange);
            var plotOB = AvaPlot1.Plot.Add.Scatter(data_X, data_Y_OilBoiler, ScottPlot.Colors.Red);
            var plotGM = AvaPlot1.Plot.Add.Scatter(data_X, data_Y_GasMotor, ScottPlot.Colors.Blue);
            var plotEB = AvaPlot1.Plot.Add.Scatter(data_X, data_Y_ElectricBoiler, ScottPlot.Colors.Green);
            plotGB.MarkerSize = 0;
            plotOB.MarkerSize = 0;
            plotGM.MarkerSize = 0;
            plotEB.MarkerSize = 0;
            AvaPlot1.Plot.Axes.SetLimitsY(min*1.2, max*1.2);
            AvaPlot1.Plot.Axes.SetLimits(8, 14.95);//one week
            // Adding Graphs labels
            ScottPlot.TickGenerators.NumericManual tickGen = new();
            for (int i = 0; i < newData.Length; i++)
            {
                if (i % 24 == 0) // Add major ticks at each day start
                {
                    string label = (period == "summer") ? $"{data_X[i]}.07" : $"{data_X[i]}.02";
                    tickGen.AddMajor(data_X[i], label);
                }
                if (i % 24 == 12)
                {
                    string hourLabel = $"12:00";
                    tickGen.AddMajor(data_X[i], hourLabel);
                }
                tickGen.AddMinor(data_X[i]);
            }
            AvaPlot1.Plot.Axes.Bottom.TickGenerator = tickGen;
            AvaPlot1.Refresh();
        }
        public void SummerPeriodButton(object sender, RoutedEventArgs e)
        {
            WinterPeriod.Background = new SolidColorBrush(Colors.Gray);
            SummerPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayExpensesContent("summer", 1);//4 date 6 heat demand
        }
        public void WinterPeriodButton(object sender, RoutedEventArgs e)
        {
            SummerPeriod.Background = new SolidColorBrush(Colors.Gray);
            WinterPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayExpensesContent("winter", 0);//0 date 2 heat demand
        }
    }
}