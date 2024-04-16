using Avalonia;
using Avalonia.Controls;

namespace SemesterProject.Views
{

    public partial class ProductionUnitsWindow : Window
    {
        public void UnitFile()
        {
            ReadFile readFile = new();
            readFile.Save();
        }
        public ProductionUnitsWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
        }
    }
}