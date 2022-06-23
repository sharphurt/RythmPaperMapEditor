using System;
using System.Windows;
using System.Windows.Controls;
using RythmPaperMapEditor.Models;
using RythmPaperMapEditor.ViewModels;
using RythmPaperMapEditor.Views.CustomControls;

namespace RythmPaperMapEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void MainWindowViewModel_OnFileLoaded()
        {
            
        }
    }
}