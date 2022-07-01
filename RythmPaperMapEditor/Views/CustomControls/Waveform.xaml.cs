using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using NAudio.Wave;
using NAudio.WaveFormRenderer;
using RythmPaperMapEditor.Models;
using RythmPaperMapEditor.ViewModels;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Pen = System.Drawing.Pen;
using PlaybackState = RythmPaperMapEditor.ViewModels.PlaybackState;
using Point = System.Drawing.Point;

namespace RythmPaperMapEditor.Views.CustomControls
{
    public partial class Waveform : UserControl
    {
        private MainWindowViewModel _viewModel;

        private List<Note> _notes;

        public event Action<List<Note>> NotesListChanged;

        public bool AutoScroll
        {
            get { return (bool)GetValue(AutoScrollProperty); }
            set { SetValue(AutoScrollProperty, value); }
        }

        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.Register("AutoScroll", typeof(bool), typeof(Waveform), new PropertyMetadata(null));

        public TrackSettings TrackSettings
        {
            get { return (TrackSettings)GetValue(TrackSettingsProperty); }
            set { SetValue(TrackSettingsProperty, value); }
        }

        public static readonly DependencyProperty TrackSettingsProperty =
            DependencyProperty.Register("TrackSettings", typeof(object), typeof(Waveform), new PropertyMetadata(null));

        public Waveform()
        {
            InitializeComponent();

            _notes = new List<Note>();

            DataContextChanged += (sender, args) =>
            {
                if (DataContext is MainWindowViewModel viewModel)
                {
                    _viewModel = viewModel;
                    _viewModel.OnFileLoaded += GenerateTimeline;
                    _viewModel.OnTrackSettingsUpdated += GenerateTimeline;
                    _viewModel.RegisterNotesListChangedEventHandler(this);
                }
            };
        }

        public void GenerateTimeline()
        {
            var rmsPeakProvider = new RmsPeakProvider(200); // e.g. 200

            var myRendererSettings = new StandardWaveFormRendererSettings
            {
                BottomPeakPen = new Pen(Color.CornflowerBlue),
                TopPeakPen = new Pen(Color.CornflowerBlue),
                BackgroundColor = Color.Empty,
                TopHeight = 32,
                BottomHeight = 32
            };

            var trackLength = GenerateGrid(TrackSettings);
            GridStack.Width = trackLength;

            myRendererSettings.Width = trackLength / 2;
            var image = RenderInThread(_viewModel.SelectedTrack.Filepath, rmsPeakProvider, myRendererSettings);

            GridStackContainer.Background = new ImageBrush(GetImageStream(image));

            var timeline = new Timeline
            {
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            timeline.DrawTimeline(_viewModel.AudioPlayer.GetLenght(), trackLength);

            TimelineContainer.Children.Clear();
            TimelineContainer.Children.Add(timeline);

            var updateIndicatorTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 1), DispatcherPriority.Render,
                (sender, args) => UpdateTimeIndicator(null, null), Dispatcher.CurrentDispatcher);
            updateIndicatorTimer.Start();
        }

        private void UpdateTimeIndicator(object sender, ElapsedEventArgs e)
        {
            if (_viewModel.PlaybackState == PlaybackState.Stopped)
                TimeIndicator.Margin = new Thickness(0);
            else
            {
                SetTimelineIndicatorPosition();

                if (AutoScroll)
                {
                    DoAutoscroll();
                }
            }
        }

        private void OnNoteAdded(Note note)
        {
            _notes.Add(note);
            NotesListChanged?.Invoke(_notes);
        }

        private void OnNoteRemoved(int timeIndex)
        {
            _notes = _notes.Where(note => note.Time == timeIndex).ToList();
            NotesListChanged?.Invoke(_notes);
        }

        private void DoAutoscroll()
        {
            var currentPosition = (_viewModel.AudioPlayer.GetPosition().TotalMilliseconds /
                                   _viewModel.AudioPlayer.GetLenght().TotalMilliseconds *
                                   GridStack.Width);

            ScrollViewer.ScrollToHorizontalOffset(currentPosition - Container.ActualWidth / 2);
        }

        private void SetTimelineIndicatorPosition()
        {
            var currentPosition = (_viewModel.AudioPlayer.GetPosition().TotalMilliseconds /
                                   _viewModel.AudioPlayer.GetLenght().TotalMilliseconds *
                                   GridStack.Width);

            TimeIndicator.Margin = new Thickness(currentPosition - TimeIndicator.ActualWidth / 2, 0, 0, 0);
        }

        private Image RenderInThread(string path, IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            var renderer = new WaveFormRenderer();
            Image image = null;
            try
            {
                using (var waveStream = new AudioFileReader(path))
                {
                    image = renderer.Render(waveStream, peakProvider, settings);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return image;
        }

        public int GenerateGrid(TrackSettings settings)
        {
            var audioLength = _viewModel.AudioPlayer.GetLenght().TotalMinutes;
            var bpm = settings.BPM * (double)settings.Scale;
            var gridElementsCount = (int)(audioLength * bpm) + 1;
            var elementGridWidth = (int)new TrackGridElementHolder(0).Width;
            var marginBetween = 50;
            var trackWidth = gridElementsCount * (elementGridWidth + marginBetween);

            for (var i = 0; i < gridElementsCount; i++)
            {
                var element = new TrackGridElementHolder(i)
                {
                    Margin = new Thickness(0, 0, marginBetween, 0)
                };

                element.NoteAdded += OnNoteAdded;
                element.NoteRemoved += OnNoteRemoved;

                GridStack.Children.Add(element);
            }

            if (audioLength != 0 && trackWidth != 0)
            {
                var margin = (trackWidth / _viewModel.AudioPlayer.GetLenght().TotalSeconds) * settings.Offset;
                GridStack.Margin = new Thickness(margin - elementGridWidth / 2f, 0, 0, 0);
            }

            return trackWidth;
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);

        public static BitmapSource GetImageStream(Image myImage)
        {
            var bitmap = new Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmpPt,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }

        private void ScrollViewer_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && AutoScroll)
                AutoScroll = false;
        }

        private void SetTimelineIndicatorToPoint(System.Windows.Point point)
        {
            var totalSeconds = _viewModel.AudioPlayer.GetLenght().TotalMilliseconds;
            var x = (ScrollViewer.ContentHorizontalOffset + point.X) / GridStack.ActualWidth;
            _viewModel.CurrentTrackPosition = TimeSpan.FromMilliseconds(x * totalSeconds);
        }

        private void TimelineContainer_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetTimelineIndicatorToPoint(e.GetPosition(this));
        }
    }
}