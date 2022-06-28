using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using NAudio.CoreAudioApi;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.WPF;

namespace RythmPaperMapEditor.Views.CustomControls
{
    public partial class PeakIndicator : UserControl
    {
        private MMDevice _device;

        public PeakIndicator()
        {
            InitializeComponent();

            int pos = 0;

            MMDeviceEnumerator de = new MMDeviceEnumerator();
            _device = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[1];
        }


        void GlDrawBar(OpenGL gl, (double x, double y, double z) origin, float W, float H,
            (double r, double g, double b) color)
        {
            gl.Begin(BeginMode.Quads);
            gl.Color(color.r, color.g, color.b);
            gl.Vertex(origin.x, origin.y, origin.z);
            gl.Vertex(origin.x + W, origin.y, origin.z);
            gl.Vertex(origin.x + W, origin.y + H, origin.z);
            gl.Vertex(origin.x, origin.y + H, origin.z);
            gl.End();
        }


        float lastValue = 0;
        float rquad = 0;



        private void OpenGLControl_OnOpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            //  Get the OpenGL instance that's been passed to us.
            OpenGL gl = args.OpenGL;

            //  Clear the color and depth buffers.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            var volume = _device.AudioMeterInformation.MasterPeakValue;
            var origin = (0, 0, 0);
            var color = (1, 0, 1);
//            Console.WriteLine(volume);

            gl.LoadIdentity();
            gl.Translate(-26f, 0.0f, -7.0f);
            GlDrawBar(gl, origin, 52 * Utils.Lerp(lastValue, volume, 0.1f), 50, color);

            //  Flush OpenGL.
            gl.Flush();
            lastValue = volume;
        }
    }
}