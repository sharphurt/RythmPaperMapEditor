using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RythmPaperMapEditor.Models
{
    public class Track : INotifyPropertyChanged
    {
        private string _friendlyName;
        private string _filepath;
        private TimeSpan _time;

        public string FriendlyName
        {
            get { return _friendlyName; }
            set
            {
                if (value == _friendlyName) return;
                _friendlyName = value;
                OnPropertyChanged(nameof(FriendlyName));
            }
        }

        public string Filepath
        {
            get { return _filepath; }
            set
            {
                if (value == _filepath) return;
                _filepath = value;
                OnPropertyChanged(nameof(Filepath));
            }
        }

        public TimeSpan Time
        {
            get { return _time; }
            set
            {
                if (value == _time) return;
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Track(string filepath, string friendlyName, TimeSpan time)
        {
            Filepath = filepath;
            FriendlyName = friendlyName;
            Time = time;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}