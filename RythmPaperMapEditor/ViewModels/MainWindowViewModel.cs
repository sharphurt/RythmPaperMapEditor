using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using NAudio.Wave;
using Newtonsoft.Json;
using RythmPaperMapEditor.Models;
using RythmPaperMapEditor.Views.CustomControls;
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
        private TimeSpan _currentTrackPosition;
        private float _currentVolume;

        private WaveStream _waveStream;

        public event Action<List<Note>> OnFileLoaded;
        public event Action<List<Note>> OnTrackSettingsUpdated;

        public AudioPlayer AudioPlayer => _audioPlayer;

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

        public TimeSpan CurrentTrackPosition
        {
            get => AudioPlayer?.GetPosition() ?? TimeSpan.Zero;
            set
            {
                AudioPlayer.SetPosition(value);
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

        private TrackSettings _appliedTrackSettings;

        private int _bpm;

        public int BPM
        {
            get => _bpm;
            set
            {
                _bpm = value;
                OnPropertyChanged(nameof(IsTrackLoaded));
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
                OnPropertyChanged(nameof(IsTrackLoaded));
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
                OnPropertyChanged(nameof(IsTrackLoaded));
                OnPropertyChanged(nameof(Offset));
            }
        }

        public bool IsTrackLoaded => SelectedTrack == null;

        public TrackSettings AppliedTrackSettings
        {
            get => _appliedTrackSettings;
            set
            {
                _appliedTrackSettings = value;
                OnPropertyChanged(nameof(AppliedTrackSettings));
            }
        }

        private bool _isAutoscrollEnabled;

        public bool IsAutoscrollEnabled
        {
            get => _isAutoscrollEnabled;
            set
            {
                _isAutoscrollEnabled = value;
                OnPropertyChanged(nameof(IsAutoscrollEnabled));
            }
        }

        public ICommand ExitApplicationCommand { get; set; }

        public ICommand RewindToStartCommand { get; set; }
        public ICommand StartPlaybackCommand { get; set; }
        public ICommand StopPlaybackCommand { get; set; }
        public ICommand OpenAudioCommand { get; set; }

        public ICommand UpdateTrackSettingsCommand { get; set; }

        public ICommand TrackControlMouseDownCommand { get; set; }
        public ICommand TrackControlMouseUpCommand { get; set; }
        public ICommand VolumeControlValueChangedCommand { get; set; }

        public ICommand SwitchAutoscrollHotkeyCommand { get; set; }

        public ICommand ExportMapCommand { get; set; }

        public ICommand OpenMapCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Note> _notes;

        public MainWindowViewModel()
        {
            PlaybackState = PlaybackState.Stopped;
            CurrentVolume = 1;

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Closing += HandleWindowClosing;

            AppliedTrackSettings = new TrackSettings(BPM, Scale, Offset);
            LoadCommands();
            InitializeUpdateTimer(1);
        }

        private void InitializeUpdateTimer(int interval)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = interval;
            timer.Elapsed += (sender, args) => OnPropertyChanged(nameof(CurrentTrackPosition));
            timer.Start();
        }

        public void RegisterNotesListChangedEventHandler(Waveform waveform)
        {
            waveform.NotesListChanged += WaveformOnNotesListChanged;
        }

        private void WaveformOnNotesListChanged(List<Note> obj)
        {
            _notes = obj;
        }

        private void LoadCommands()
        {
            ExitApplicationCommand = new RelayCommand(ExitApplication, CanExitApplication);

            RewindToStartCommand = new RelayCommand(RewindToStart, CanRewindToStart);
            StartPlaybackCommand = new RelayCommand(StartPlayback, CanStartPlayback);
            StopPlaybackCommand = new RelayCommand(StopPlayback, CanStopPlayback);
            OpenAudioCommand = new RelayCommand(OpenFile, CanOpenFile);

            UpdateTrackSettingsCommand = new RelayCommand(UpdateTrackSettings, CanUpdateTrackSettings);

            TrackControlMouseDownCommand = new RelayCommand(TrackControlMouseDown, CanTrackControlMouseDown);
            TrackControlMouseUpCommand = new RelayCommand(TrackControlMouseUp, CanTrackControlMouseUp);
            VolumeControlValueChangedCommand =
                new RelayCommand(VolumeControlValueChanged, CanVolumeControlValueChanged);
            SwitchAutoscrollHotkeyCommand = new RelayCommand(SwitchAutoscroll, CanSwitchAutoscroll);

            ExportMapCommand = new RelayCommand(SaveNotesList, CanSaveNotesList);

            OpenMapCommand = new RelayCommand(OpenMap, CanOpenMap);
        }

        private bool CanOpenMap(object obj)
        {
            return true;
        }

        private void OpenMap(object obj)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Json file|*.json"
            };

            var result = ofd.ShowDialog();

            if (result == true)
            {
                Map map = null;
                
                try
                {
                    map = JsonConvert.DeserializeObject<Map>(File.ReadAllText(ofd.FileName));
                }
                catch (Exception e)
                {
                    MessageBox.Show("Map not found in this file");
                }

                if (map == null)
                {
                    MessageBox.Show("Map not found in this file");
                    return;
                }

                if (File.Exists(map.Audio))
                {
                    Title = map.Name;
                    BPM = map.Bpm;
                    Scale = map.Scale;
                    Offset = map.Offset;
                    AppliedTrackSettings = new TrackSettings(map.Bpm, map.Scale, map.Offset);

                    var duration = new AudioFileReader(map.Audio).TotalTime;
                    SelectedTrack = new Track(map.Audio, Path.GetFileNameWithoutExtension(map.Audio), duration);

                    _notes = map.Notes;
                    InitializeAudioPlayer();
                    OnFileLoaded?.Invoke(map.Notes);
                }
                else
                {
                    MessageBox.Show("Link to audio file missed");
                }
            }
        }

        private bool CanSaveNotesList(object o)
        {
            return _notes != null && _notes.Count > 0;
        }

        private void SaveNotesList(object o)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Json file|*.json";
            var result = sfd.ShowDialog();

            if (result == true)
            {
                var map = new Map(Title, BPM, Scale, Offset, SelectedTrack.Filepath,
                    _notes.OrderBy(note => note.Time).ToList());

                File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(map));
            }
        }

        private bool CanSwitchAutoscroll(object o)
        {
            return true;
        }

        private void SwitchAutoscroll(object o)
        {
            IsAutoscrollEnabled = !IsAutoscrollEnabled;
        }

        private bool CanUpdateTrackSettings(object obj)
        {
            return true;
        }

        private void UpdateTrackSettings(object obj)
        {
            AppliedTrackSettings = new TrackSettings(BPM, Scale, Offset);
            OnTrackSettingsUpdated?.Invoke(_notes);
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
            CurrentTrackLenght = _audioPlayer.GetLenght().TotalSeconds;

            WaveStream = _audioPlayer.AudioFileReader;
        }

        private void OpenFile(object p)
        {
            var openDialog = new ImportTrackForm();
            var result = openDialog.ShowDialog();
            if (result == true)
            {
                SelectedTrack = openDialog.Track;
                BPM = openDialog.TrackSettings.BPM;
                Scale = openDialog.TrackSettings.Scale;
                Offset = openDialog.TrackSettings.Offset;
                AppliedTrackSettings = openDialog.TrackSettings;

                Title = openDialog.Track.FriendlyName;

                InitializeAudioPlayer();
                OnFileLoaded?.Invoke(new List<Note>());
            }
        }

        public bool CanOpenFile(object p)
        {
            return true;
        }

        private void HandleWindowClosing(object sender, CancelEventArgs e)
        {
            _audioPlayer?.Dispose();
        }

        private void HandlePlaybackStop()
        {
            PlaybackState = PlaybackState.Stopped;
            _audioPlayer.SetPosition(TimeSpan.Zero);
            CommandManager.InvalidateRequerySuggested();
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
            _audioPlayer.SetPosition(TimeSpan.Zero);
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

            /*
            _audioPlayer.SetPosition(CurrentTrackPosition);
            */
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