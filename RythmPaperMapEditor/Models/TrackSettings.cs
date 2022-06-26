namespace RythmPaperMapEditor.Models
{
    public class TrackSettings
    {
        public int BPM { get; }
        
        public int Scale { get; }
        
        public double Offset { get; }

        public TrackSettings(int bpm, int scale, double offset)
        {
            BPM = bpm;
            Scale = scale;
            Offset = offset;
        }
    }
}