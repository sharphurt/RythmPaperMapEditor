using System.Windows;
using Microsoft.Win32;
using NAudio.Wave;
using RythmPaperMapEditor.Models;
using RythmPaperMapEditor.ViewModels;

namespace RythmPaperMapEditor.Views.Forms
{
    public partial class ImportTrackForm : Window
    {
        private readonly ImportTrackFormViewModel _viewModel;

        public TrackSettings TrackSettings { get; private set; }

        public Track Track { get; private set; }

        public ImportTrackForm()
        {
            InitializeComponent();
            _viewModel = (ImportTrackFormViewModel)DataContext;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            TrackSettings = new TrackSettings(_viewModel.BPM, _viewModel.Scale, _viewModel.Offset);
            Track = _viewModel.Track;
            DialogResult = true;
        }

        private void ImportTrackForm_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.OpenFile(this);
        }
    }
}