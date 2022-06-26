using System;
using System.ComponentModel;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using NAudio.Wave;
using RythmPaperMapEditor.Models;
using RythmPaperMapEditor.Views.Forms;
using RythmPaperMapEditor.Wrappers;

namespace RythmPaperMapEditor.ViewModels
{
    public enum PlaybackState
    {
        Playing,
        Stopped,
        Paused
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private PlaybackState _playbackState;
        private Track _selectedTrack;

        private AudioPlayer _audioPlayer;

        private string _title;
        private double _currentTrackLenght;
        private double _currentTrackPosition;
        private float _currentVolume;

        private WaveStream _waveStream;

        public event Action OnFileLoaded;

        public WaveStream WaveStream
        {
            get => _waveStream;
            set
            {
                _waveStream = value;
                OnPropertyChanged(nameof(WaveStream));
            }
        }

        public PlaybackState PlaybackState
        {
            get => _playbackState;
            set
            {
                _playbackState = value;
                OnPropertyChanged(nameof(PlaybackState));
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public float CurrentVolume
        {
            get => _currentVolume;
            set
            {
                if (value == _currentVolume) return;
                _currentVolume = value;
                OnPropertyChanged(nameof(CurrentVolume));
            }
        }

        public double CurrentTrackLenght
        {
            get => _currentTrackLenght;
            set
            {
                if (value.Equals(_currentTrackLenght)) return;
                _currentTrackLenght = value;
                OnPropertyChanged(nameof(CurrentTrackLenght));
            }
        }

        public double CurrentTrackPosition
        {
            get => _currentTrackPosition;
            set
            {
                if (value.Equals(_currentTrackPosition)) return;
                _currentTrackPosition = value;
                OnPropertyChanged(nameof(CurrentTrackPosition));
            }
        }

        public Track SelectedTrack
        {
            get => _selectedTrack;
            set
            {
                if (Equals(value, _selectedTrack)) return;
                _selectedTrack = value;
                OnPropertyChanged(nameof(SelectedTrack));
            }
        }

        private TrackSettings _trackSettings;

        public TrackSettings TrackSettings
        {
            get => _trackSettings;
            set
            {
                _trackSettings = value;
                OnPropertyChanged(nameof(TrackSettings));
            }
        }
        
        public ICommand ExitApplicationCommand { get; set; }

        public ICommand RewindToStartCommand { get; set; }
        public ICommand StartPlaybackCommand { get; set; }
        public ICommand StopPlaybackCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }

        public ICommand TrackControlMouseDownCommand { get; set; }
        public ICommand TrackControlMouseUpCommand { get; set; }
        public ICommand VolumeControlValueChangedCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            PlaybackState = PlaybackState.Stopped;
            CurrentVolume = 1;

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Closing += HandleWindowClosing;

            LoadCommands();
            InitializeUpdateTimer(1);
        }

        private void InitializeUpdateTimer(int interval)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = interval;
            timer.Elapsed += HandleTimerOver;
            timer.Start();
        }

        private void LoadCommands()
        {
            ExitApplicationCommand = new RelayCommand(ExitApplication, CanExitApplication);

            RewindToStartCommand = new RelayCommand(RewindToStart, CanRewindToStart);
            StartPlaybackCommand = new RelayCommand(StartPlayback, CanStartPlayback);
            StopPlaybackCommand = new RelayCommand(StopPlayback, CanStopPlayback);
            OpenFileCommand = new RelayCommand(OpenFile, CanOpenFile);

            TrackControlMouseDownCommand = new RelayCommand(TrackControlMouseDown, CanTrackControlMouseDown);
            TrackControlMouseUpCommand = new RelayCommand(TrackControlMouseUp, CanTrackControlMouseUp);
            VolumeControlValueChangedCommand =
                new RelayCommand(VolumeControlValueChanged, CanVolumeControlValueChanged);
        }

        private void InitializeAudioPlayer()
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.Stop();
                _audioPlayer.Dispose();
            }
            
            _audioPlayer = new AudioPlayer(SelectedTrack.Filepath, CurrentVolume);

            _audioPlayer.OnPlaybackPause += HandlePlaybackPause;
            _audioPlayer.OnPlaybackResume += HandlePlaybackResume;
            _audioPlayer.OnPlaybackStop += HandlePlaybackStop;
            CurrentTrackLenght = _audioPlayer.GetLenghtInSeconds();

            WaveStream = _audioPlayer.AudioFileReader;
        }

        private void OpenFile(object p)
        {
            var openDialog = new ImportTrackForm();
            var result = openDialog.ShowDialog();
            if (result == true)
            {
                SelectedTrack = openDialog.Track;
                TrackSettings = openDialog.TrackSettings;
                
                InitializeAudioPlayer();
                OnFileLoaded?.Invoke();
            }
        }

        public bool CanOpenFile(object p)
        {
            return true;
        }

        private void UpdateSeekBar()
        {
            if (PlaybackState == PlaybackState.Playing)
            {
                CurrentTrackPosition = _audioPlayer.GetPositionInSeconds();
            }
        }

        private void HandleTimerOver(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateSeekBar();
        }

        private void HandleWindowClosing(object sender, CancelEventArgs e)
        {
            _audioPlayer?.Dispose();
        }

        private void HandlePlaybackStop()
        {
            PlaybackState = PlaybackState.Stopped;
            CommandManager.InvalidateRequerySuggested();
            CurrentTrackPosition = 0;
        }

        private void HandlePlaybackResume()
        {
            PlaybackState = PlaybackState.Playing;
        }

        private void HandlePlaybackPause()
        {
            PlaybackState = PlaybackState.Paused;
        }

        private void ExitApplication(object p)
        {
            _audioPlayer?.Dispose();
            Application.Current.Shutdown();
        }

        private bool CanExitApplication(object p)
        {
            return true;
        }

        private void RewindToStart(object p)
        {
            _audioPlayer.SetPosition(0);
        }

        private bool CanRewindToStart(object p)
        {
            return _playbackState == PlaybackState.Playing;
        }

        private void StartPlayback(object p)
        {
            if (SelectedTrack == null)
                return;

            if (_audioPlayer == null)
                InitializeAudioPlayer();

            _audioPlayer.TogglePlayPause(CurrentVolume);
        }

        private bool CanStartPlayback(object p) => SelectedTrack != null;

        private void StopPlayback(object p)
        {
            if (_audioPlayer == null) return;

            _audioPlayer.Stop();
        }

        private bool CanStopPlayback(object p)
        {
            return _playbackState == PlaybackState.Playing || _playbackState == PlaybackState.Paused;
        }

        private void TrackControlMouseDown(object p)
        {
            _audioPlayer?.Pause();
        }

        private void TrackControlMouseUp(object p)
        {
            if (_audioPlayer == null) return;

            _audioPlayer.SetPosition(CurrentTrackPosition);
            _audioPlayer.Play(NAudio.Wave.PlaybackState.Paused, CurrentVolume);
        }

        private bool CanTrackControlMouseDown(object p)
        {
            return _playbackState == PlaybackState.Playing;
        }

        private bool CanTrackControlMouseUp(object p)
        {
            return _playbackState == PlaybackState.Paused;
        }

        private void VolumeControlValueChanged(object p)
        {
            _audioPlayer?.SetVolume(CurrentVolume);
        }

        private bool CanVolumeControlValueChanged(object p)
        {
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}