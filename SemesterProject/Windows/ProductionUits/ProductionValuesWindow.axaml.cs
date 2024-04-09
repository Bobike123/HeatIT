using Avalonia;
using System;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Diagnostics;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Scripting;

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