using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RythmPaperMapEditor.Views
{
    public partial class Timeline : UserControl
    {
        public Timeline()
        {
            InitializeComponent();
        }

        public void DrawTimeline(TimeSpan length, double width)
        {
            Grid.Children.Clear();
            var brush = new SolidColorBrush(Colors.Silver);
            // Draw the bottom border
            var bottomBorder = new Border
            {
                Background = brush,
                Height = 1,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            Grid.Children.Add(bottomBorder);

            var totalSeconds = length.TotalSeconds;

            var pixelsPerSecond = width / totalSeconds;

            for (var i = 0; i < totalSeconds; i++)
            {
                var margin = pixelsPerSecond * i;
                var tick = new Border
                {
                    Width = 1,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = brush,
                    Margin = new Thickness(margin, 0, 0, 0)
                };
                Grid.Children.Add(tick);

                for (int j = 1; j < 5; j++)
                {
                    var minorTick = new Border
                    {
                        Height = 10,
                        Width = 1,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Background = brush,
                        Margin = new Thickness(margin + (j * pixelsPerSecond / 5), 0, 0, 0)
                    };
                    Grid.Children.Add(minorTick);
                }

                // Add time label
                var time = new TextBlock();
                time.VerticalAlignment = VerticalAlignment.Top;
                time.HorizontalAlignment = HorizontalAlignment.Left;
                time.Foreground = brush;
                time.FontSize = FontSize;
                var ts = TimeSpan.FromSeconds(i);
                time.Text = ts.TotalHours >= 1 ? ts.ToString(@"h\:mm\:ss") : ts.ToString(@"mm\:ss");
                time.Margin = new Thickness(margin + 5, 0, 0, 0);
                Grid.Children.Add(time);
            }
        }

        public void Redraw(TimeSpan length, double width)
        {
            var totalSeconds = length.TotalSeconds;

            var pixelsPerSecond = width / totalSeconds;

            var counter = 1;

            for (var i = 0; i < totalSeconds; i++)
            {
                var margin = pixelsPerSecond * i;
                ((Border)Grid.Children[counter]).Margin = new Thickness(margin, 0, 0, 0);

                for (int j = 1; j < 5; j++)
                {
                    ((Border)Grid.Children[counter + j]).Margin =
                        new Thickness(margin + (j * pixelsPerSecond / 5), 0, 0, 0);
                }

                ((TextBlock)Grid.Children[counter + 5]).Margin = new Thickness(margin + 5, 0, 0, 0);
                
                counter += 6;
            }
        }
    }
}