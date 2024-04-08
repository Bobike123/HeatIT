using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;


namespace SemesterProject
{
    public class AssetManagerViewModel
    {
        public AssetManager.ProductionUnit[] ProductionUnits { get; } = AssetManager.ProductionUnits;
    }

    public class AssetManagerView : Window
    {
        public AssetManagerView()
        {
            InitializeComponent();
            DataContext = new AssetManagerViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
