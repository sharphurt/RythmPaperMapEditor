using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
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

namespace RythmPaperMapEditor.Views.CustomControls
{
    public partial class Waveform : UserControl
    {
        private double _audioLength;

        private MainWindowViewModel _viewModel;

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

            DataContextChanged += (sender, args) =>
            {
                if (DataContext is MainWindowViewModel viewModel)
                {
                    _viewModel = viewModel;
                    _viewModel.OnFileLoaded += GenerateTimeline;
                }
            };
        }

        private void GenerateTimeline()
        {
            var rmsPeakProvider = new RmsPeakProvider(200); // e.g. 200

            var myRendererSettings = new StandardWaveFormRendererSettings();
            myRendererSettings.BottomPeakPen = new Pen(Color.CornflowerBlue);
            myRendererSettings.TopPeakPen = new Pen(Color.CornflowerBlue);
            myRendererSettings.BackgroundColor = Color.Empty;
            myRendererSettings.TopHeight = 32;
            myRendererSettings.BottomHeight = 32;

            _audioLength = _viewModel.CurrentTrackLenght;
            var trackLength = GenerateGrid(TrackSettings);
            GridStack.Width = trackLength;

            myRendererSettings.Width = trackLength;
            var image = RenderInThread(_viewModel.SelectedTrack.Filepath, rmsPeakProvider, myRendererSettings);
            FinishRender(image);

            var timer = new Timer();
            timer.Interval = 1;
            Console.WriteLine(timer.Interval);
            timer.Elapsed += UpdateTimeIndicator;
            timer.Start();
        }

        private void UpdateTimeIndicator(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (_viewModel.PlaybackState == PlaybackState.Stopped)
                    TimeIndicator.Margin = new Thickness(0);
                else if (_viewModel.PlaybackState == PlaybackState.Playing)
                {
                    var currentPosition = (_viewModel.CurrentTrackPosition / _viewModel.CurrentTrackLenght *
                                           GridStack.Width);
                    TimeIndicator.Margin = new Thickness(currentPosition, 0, 0, 0);
                }
            });
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

        private void FinishRender(Image image)
        {
            Grid.Width = image.Width;
            Grid.Background = new ImageBrush(GetImageStream(image));
        }

        public int GenerateGrid(TrackSettings settings)
        {
            var grid = GridStack;
            var bpmInSeconds = settings.BPM / (double) settings.Scale / 60;
            var gridElementsCount = (int)(_audioLength / bpmInSeconds);
            var elementGridWidth = 2;
            var marginBetween = 50;
            var trackWidth = gridElementsCount * (elementGridWidth + marginBetween);

            for (var i = 0; i < gridElementsCount; i++)
            {
                var element = new Button
                {
                    Width = elementGridWidth
                };
                element.Margin = new Thickness(0, 0, marginBetween, 0);
                grid.Children.Add(element);
            }

            GridStack.Margin = new Thickness((trackWidth / _audioLength) * settings.Offset, 0, 0, 0);

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

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }
    }
}