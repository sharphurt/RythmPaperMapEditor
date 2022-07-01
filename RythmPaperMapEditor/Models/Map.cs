using System.Collections.Generic;
using Newtonsoft.Json;

namespace RythmPaperMapEditor.Models
{
    public class Map
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("bpm")]
        public int Bpm;

        [JsonProperty("scale")]
        public int Scale;

        [JsonProperty("offset")]
        public double Offset;

        [JsonProperty("audio")]
        public string Audio;

        [JsonProperty("notes")]
        public List<Note> Notes;

        public Map(string name, int bpm, int scale, double offset, string audio, List<Note> notes)
        {
            Name = name;
            Bpm = bpm;
            Scale = scale;
            Offset = offset;
            Audio = audio;
            Notes = notes;
        }
    }
}