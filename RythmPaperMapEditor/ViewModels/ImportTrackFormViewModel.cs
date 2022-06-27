using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using NAudio.Wave;
using RythmPaperMapEditor.Models;
using RythmPaperMapEditor.Views.Forms;

namespace RythmPaperMapEditor.ViewModels
{
    public class ImportTrackFormViewModel : INotifyPropertyChanged
    {
        public Track Track { get; private set; }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _path;

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        private string _duration;

        public string Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        private int _bpm;

        public int BPM
        {
            get => _bpm;
            set
            {
                _bpm = value;
                OnPropertyChanged(nameof(BPM));
            }
        }

        private int _scale;

        public int Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                OnPropertyChanged(nameof(Scale));
            }
        }

        private double _offset;

        public double Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                OnPropertyChanged(nameof(Offset));
            }
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public void OpenFile(Window window)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Audio files (*.wav, *.mp3, *.wma, *.ogg, *.flac) | *.wav; *.mp3; *.wma; *.ogg; *.flac"
            };
            var result = ofd.ShowDialog();
            if (result == true)
            {
                Path = ofd.FileName;
                Name = ofd.SafeFileName.Remove(ofd.SafeFileName.Length - 4);
                var duration = new AudioFileReader(Path).TotalTime;
                Duration = duration.ToString("g");
                Track = new Track(Path, Name, duration);
            }
            else
            {
                CloseWindow(window);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}