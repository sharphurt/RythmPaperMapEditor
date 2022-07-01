using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RythmPaperMapEditor.Enums;

namespace RythmPaperMapEditor.Models
{
    public class Note
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public NoteType Type { get; }

        public NoteParameters Parameters { get; }

        public int Time { get; }
        
        public Note(NoteType type, NoteParameters parameters, int time)
        {
            Type = type;
            Parameters = parameters;
            Time = time;
        }
    }
}