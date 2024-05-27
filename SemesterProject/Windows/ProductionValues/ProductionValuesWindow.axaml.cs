using Avalonia;
using System.IO;
using Avalonia.Media;
using Avalonia.Controls;
using ScottPlot.Avalonia;
using Avalonia.Interactivity;


namespace SemesterProject.Views
{

    public partial class ProductionValuesWindow : Window
    {
        public ProductionValuesWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
        }

        public void DisplayGraphContentElectricityPrices(int[] columns, string period)
        {
            AvaPlot avaPlot1 = this.Find<AvaPlot>("AvaPlot1")!;//initializes the graph

            avaPlot1.Plot.Clear();
            //clears the graph if any previous information was displayed on it
            string[][] newData = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), columns);
            //newData is siplified data that can be used for the graph
            double min = double.MaxValue, max = double.MinValue, sum = 0;
            double[] data_X = new double[newData.Length], data_Y = new double[newData.Length];
            double count = 0;//cout for the 24 hours of the day used for the x axis
            for (int i = 0; i < newData.Length; i++)
            {
                data_X[i] = double.Parse(newData[i][0]) + count * 0.041;//creates the x axis
                data_Y[i] = double.Parse(newData[i][1]);                //creates the y axis
                if (double.Parse(newData[i][1]) > max) max = double.Parse(newData[i][1]);//calculates the maximum variable
                if (double.Parse(newData[i][1]) < min) min = double.Parse(newData[i][1]);//calculates the minimum variable
                count = (count + 1) % 24;
            }
            //modifies the highest lowest and average data from the axaml file
            sum = (max + min) / 2;
            HighestElectricity.Text = ($"{max:00.00} DKK");
            LowestElectricity.Text = ($"{min:00.00} DKK");
            AverageElectricity.Text= ($"{sum:00.00} DKK");

            //modifies the title of the graph depending on the time of the year
            if (period == "summer") avaPlot1.Plot.Title("Price for MWH for Summer Period");
            else avaPlot1.Plot.Title("Price for MWH for Winter Period");
            avaPlot1.Plot.XLabel("Days");
            avaPlot1.Plot.YLabel("DKK");
            var plot = avaPlot1.Plot.Add.Scatter(data_X, data_Y);
            plot.MarkerSize = 0;
            avaPlot1.Plot.Axes.SetLimitsX(8, 14.95);//one week
            avaPlot1.Plot.Axes.SetLimitsY(0,1700);//comparation of the two periods
            avaPlot1.Refresh();
        }
        public void DisplayGraphContentElectricity(int periodInt, string period)
        {
            AssetManager assetManager = new();
            assetManager.LoadChanged();
            AvaPlot avaPlot2 = this.Find<AvaPlot>("AvaPlot2")!;//initializes the graph
            avaPlot2.Plot.Clear();

            string[][] newData = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), [0, 4]);

            double[][] datesData = Optimizer.ConvertToDoubleArray(newData);
            string[][] heatDemand = SourceDataManager.CSVDisplayGraph(Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager", "data.csv"), [2, 6]);
            double[][] heatDemandDouble = Optimizer.ConvertToDoubleArray(heatDemand);
            double[] operatingPoint = new double[heatDemandDouble.Length];
            double[] data_X = new double[datesData.Length];
            double[] data_Y_GasBoiler = new double[newData.Length];
            double[] data_Y_OilBoiler = new double[newData.Length];
            double[] data_Y_GasMotor = new double[newData.Length];
            double[] data_Y_ElectricBoiler = new double[newData.Length];
            double elmax = double.MinValue, elmin = double.MaxValue, elaverage = 0;
            double count = 0;
            double gasBoilerEle = double.Parse(assetManager.productionUnits[0].MaxElectricity!);
            double oilBoilerEle = double.Parse(assetManager.productionUnits[1].MaxElectricity!);
            double gasMotorEle = double.Parse(assetManager.productionUnits[2].MaxElectricity!);
            double electricBoilerEle = double.Parse(assetManager.productionUnits[3].MaxElectricity!);
            double gasBoilerHeat = double.Parse(assetManager.productionUnits[0].MaxHeat!);
            double oilBoilerHeat = double.Parse(assetManager.productionUnits[1].MaxHeat!);
            double gasMotorHeat = double.Parse(assetManager.productionUnits[2].MaxHeat!);
            double electricBoilerHeat = double.Parse(assetManager.productionUnits[3].MaxHeat!);
            int unit = 0;
            int nonZeroCount = 0;

            for (int x = 0; x < heatDemandDouble.Length; x++)
            {
                double heatDemandValue = heatDemandDouble[x][periodInt];
                operatingPoint[x] = 0;
                data_X[x] = datesData[x][periodInt] + count * 0.041; //creates the x axis
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
                        data_Y_GasMotor[x] = gasMotorEle * operatingPoint[x];
                        heatDemandDouble[x][periodInt] -= gasMotorHeat;
                        operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                        data_Y_GasBoiler[x] = gasBoilerEle * operatingPoint[x];
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
                            data_Y_GasMotor[x] = gasBoilerEle * operatingPoint[x];
                            data_Y_GasBoiler[x] = gasBoilerEle * operatingPoint[x];
                            heatDemandDouble[x][periodInt] -= gasBoilerHeat;
                            heatDemandDouble[x][periodInt] -= gasMotorHeat;
                            operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                            data_Y_OilBoiler[x] = oilBoilerEle * operatingPoint[x];
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
                                data_Y_GasMotor[x] = gasMotorEle * operatingPoint[x];
                                data_Y_GasBoiler[x] = gasBoilerEle * operatingPoint[x];
                                data_Y_OilBoiler[x] = oilBoilerEle * operatingPoint[x];
                                heatDemandDouble[x][periodInt] -= gasBoilerHeat;
                                heatDemandDouble[x][periodInt] -= gasMotorHeat;
                                heatDemandDouble[x][periodInt] -= oilBoilerHeat;
                                operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                                data_Y_ElectricBoiler[x] = electricBoilerEle * operatingPoint[x];
                            }
                            else
                            {
                                heatDemandValue -= electricBoilerEle;
                            }
                        }
                    }
                }
                else
                {
                    unit = 2;
                    operatingPoint = Optimizer.OperatingPoint(heatDemandDouble, assetManager, periodInt, unit);
                    data_Y_GasMotor[x] = gasMotorEle * operatingPoint[x];
                }
                double totalExpense = data_Y_GasBoiler[x] + data_Y_OilBoiler[x] + data_Y_GasMotor[x] + data_Y_ElectricBoiler[x];
                if (totalExpense > elmax) elmax = totalExpense;
                if (totalExpense < elmin) elmin = totalExpense;
                
                if (totalExpense > 0)
                {
                    elaverage += totalExpense;
                    nonZeroCount++;
                }
            }
            elaverage =  (elmax+elmin)/2;

            elhighestTextBlock.Text = $"{elmax:0.00} MWh";
            ellowestTextBlock.Text = $"{elmin:0.00} MWh";
            elaverageTextBlock.Text = $"{elaverage:0.00} MWh";

            if (period == "summer")
                avaPlot2.Plot.Title($"Electricity Production Graph for Summer Period");
            else
                avaPlot2.Plot.Title($"Electricity Production Graph for Winter Period");

            avaPlot2.Plot.XLabel("Days", size: 15);
            avaPlot2.Plot.YLabel("KG/MWh", size: 15);
            var plotGB = avaPlot2.Plot.Add.Scatter(data_X, data_Y_GasBoiler, ScottPlot.Colors.Orange);
            var plotOB = avaPlot2.Plot.Add.Scatter(data_X, data_Y_OilBoiler, ScottPlot.Colors.Red);
            var plotGM = avaPlot2.Plot.Add.Scatter(data_X, data_Y_GasMotor, ScottPlot.Colors.Blue);
            var plotEB = avaPlot2.Plot.Add.Scatter(data_X, data_Y_ElectricBoiler, ScottPlot.Colors.Green);
            plotGB.MarkerSize = 0;
            plotOB.MarkerSize = 0;
            plotGM.MarkerSize = 0;
            plotEB.MarkerSize = 0;
            avaPlot2.Plot.Axes.SetLimitsX(8, 14.95);
            avaPlot2.Plot.Axes.SetLimitsY(-10, 10);
            avaPlot2.Refresh();
        }
        public void SummerPeriodButton(object sender, RoutedEventArgs args)
        {
            WinterPeriod.Background = new SolidColorBrush(Colors.Gray);
            SummerPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayGraphContentElectricityPrices([4, 7], "summer");//4 date 6 heat demand
            DisplayGraphContentElectricity(1, "summer");
        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Colors.Gray);
            WinterPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            DisplayGraphContentElectricityPrices([0, 3], "winter");//0 date 2 heat demand
            DisplayGraphContentElectricity(0, "winter");
        }
    }

}