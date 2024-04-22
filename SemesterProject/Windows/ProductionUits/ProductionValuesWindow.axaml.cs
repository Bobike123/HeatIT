using Avalonia;
using Avalonia.Controls;

namespace SemesterProject.Views
{

    public partial class ProductionUnitsWindow : Window
    {
        public ProductionUnitsWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
            UnitFile();
        }
        public void UnitFile()
        {
            ReadFile readFile = new();
            readFile.Load();
            TableContent();
        }
        public void TableContent()
        {
            //Make so that you can access the data from the folder

            GasBoiler.Text="something.ToString();";
            GBMH.Text="something.ToString();";
            GBME.Text="something.ToString();";
            GBPC.Text="something.ToString();";
            GBCO2.Text="something.ToString();";
            GBFT.Text="something.ToString();";

            ElectricBoiler.Text="something.ToString();";
            EBMH.Text="something.ToString();";
            EBME.Text="something.ToString();";
            EBPC.Text="something.ToString();";
            EBCO2.Text="something.ToString();";
            EBFT.Text="something.ToString();";

            GasMotor.Text="something.ToString();";
            GMMH.Text="something.ToString();";
            GMME.Text="something.ToString();";
            GMPC.Text="something.ToString();";
            GMCO2.Text="something.ToString();";
            GMFT.Text="something.ToString();";

            OilBoiler.Text="something.ToString();";
            OBMH.Text="something.ToString();";
            OBME.Text="something.ToString();";
            OBPC.Text="something.ToString();";
            OBCO2.Text="something.ToString();";
            OBFT.Text="something.ToString();";
        }
    }
}