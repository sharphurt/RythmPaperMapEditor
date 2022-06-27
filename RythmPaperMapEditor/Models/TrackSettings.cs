using System.Runtime.InteropServices;

namespace RythmPaperMapEditor.Models
{
    public class TrackSettings
    {
        public int BPM { get; set; }
        
        public int Scale { get; set; }
        
        public double Offset { get; set; }

        public TrackSettings(int bpm, int scale, double offset)
        {
            BPM = bpm;
            Scale = scale;
            Offset = offset;
        }
        
        public bool IsInvalid => BPM == 0 || Scale == 0;
    }
}