using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RythmPaperMapEditor.Models
{
    public class Track : INotifyPropertyChanged
    {
        private string _friendlyName;
        private string _filepath;
        private double _length;

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

        public double Length
        {
            get { return _length; }
            set
            {
                if (value == _length) return;
                _length = value;
                OnPropertyChanged(nameof(Length));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Track(string filepath, string friendlyName, double length)
        {
            Filepath = filepath;
            FriendlyName = friendlyName;
            Length = length;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}