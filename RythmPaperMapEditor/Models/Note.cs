using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RythmPaperMapEditor.Enums;

namespace RythmPaperMapEditor.Models
{
    public class Note
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type")]
        public NoteType Type { get; }
        
        [JsonProperty("params")]
        public NoteParameters Parameters { get; }

        [JsonProperty("time")]
        public int Time { get; }
        
        public Note(NoteType type, NoteParameters parameters, int time)
        {
            Type = type;
            Parameters = parameters;
            Time = time;
        }
    }
}