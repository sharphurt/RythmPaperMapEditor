using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using RythmPaperMapEditor.Models;
using RythmPaperMapEditor.ViewModels;
using RythmPaperMapEditor.Views.CustomControls;

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

            foreach (var key in Data.Data.NoteIcons.Keys)
            {
                var noteCard = new ItemCard(key);
                NoteItemsContainer.Children.Add(noteCard);
            }
        }

        private void MainWindowViewModel_OnFileLoaded(List<Note> _)
        {
            BpmTextBox.Text = ((MainWindowViewModel)DataContext).BPM.ToString();
            ScaleTextBox.Text = ((MainWindowViewModel)DataContext).Scale.ToString();
            OffsetTextBox.Text = ((MainWindowViewModel)DataContext).Offset.ToString(CultureInfo.InvariantCulture);
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainGrid.Focus();
        }
    }
}