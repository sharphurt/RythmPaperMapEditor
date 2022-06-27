using System.Globalization;
using System.Windows;
using RythmPaperMapEditor.ViewModels;

namespace RythmPaperMapEditor.Views.Forms
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
            BpmTextBox.Text = ((MainWindowViewModel)DataContext).BPM.ToString();
            ScaleTextBox.Text = ((MainWindowViewModel)DataContext).Scale.ToString();
            OffsetTextBox.Text = ((MainWindowViewModel)DataContext).Offset.ToString(CultureInfo.InvariantCulture);
        }
    }
}