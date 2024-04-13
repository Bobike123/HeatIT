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
        public void DisplayCSVContent()
        {
            int[] columns = [0,1,4,5,6];
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataManager\\data.csv");
            SourceDataManager.CSVDisplayGraph(csvFilePath, columns );
        }


        public void HourButtonCommand(object sender, RoutedEventArgs args)
        {
            if (HourButton.IsChecked == true)
            {
                string[] data= [];
                data=["1","2","3"];
                //HourGraph display
                DisplayCSVContent();
                DayButton.IsChecked = false; WeekButton.IsChecked = false; MonthButton.IsChecked = false;   MaxButton.IsChecked = false;
             
            }
        }
        
        public void DayButtonCommand(object sender, RoutedEventArgs args)
        {
            if (DayButton.IsChecked == true)
            {
               
                //DayGraph display
                DisplayCSVContent();
                HourButton.IsChecked = false; WeekButton.IsChecked = false; MonthButton.IsChecked = false;   MaxButton.IsChecked = false;
              
            }

        }
        public void WeekButtonCommand(object sender, RoutedEventArgs args)
        {
            if (WeekButton.IsChecked == true)
            {
               
                //WeekGraph display
                DisplayCSVContent();
                HourButton.IsChecked = false; DayButton.IsChecked = false; MonthButton.IsChecked = false;   MaxButton.IsChecked = false;
               
            }
        }
        public void MonthButtonCommand(object sender, RoutedEventArgs args)
        {
            if (MonthButton.IsChecked == true)
            {
               
                //MonthGraph display
                DisplayCSVContent();
                HourButton.IsChecked = false; DayButton.IsChecked = false; WeekButton.IsChecked = false;   MaxButton.IsChecked = false;
              
            }
        }
        public void MaxButtonCommand(object sender, RoutedEventArgs args)
        {

            if (MaxButton.IsChecked == true)
            {
                
                //MaxGraph display
                DisplayCSVContent();
                HourButton.IsChecked = false; DayButton.IsChecked = false; WeekButton.IsChecked = false; MonthButton.IsChecked = false;  
              
            }
        }
    }
}