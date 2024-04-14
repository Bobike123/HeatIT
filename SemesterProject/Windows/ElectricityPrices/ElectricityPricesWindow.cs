using Avalonia;
using System.IO;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Diagnostics;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace SemesterProject.Views
{

    public partial class ElectriciyPricesWindow : Window
    {
        public ElectriciyPricesWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
        }

        public void DisplayCSVContent()
        {
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager\\data.csv");
            //SourceDataManager.CSVDisplayGraph(csvFilePath);
        }


        public void SummerPeriodButton(object sender, RoutedEventArgs args)
        {
            WinterPeriod.Background = new SolidColorBrush(Colors.Gray);
            SummerPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            //DisplayCSVContent([4, 5, 6, 7],"summer");
        }

        public void WinterPeriodButton(object sender, RoutedEventArgs args)
        {
            SummerPeriod.Background = new SolidColorBrush(Colors.Gray);
            WinterPeriod.Background = new SolidColorBrush(Color.FromRgb(207, 3, 3));
            //DisplayCSVContent([0, 1, 2, 3],"winter");
        }
    
    }
}