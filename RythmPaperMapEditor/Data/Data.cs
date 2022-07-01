using System.Collections.Generic;
using RythmPaperMapEditor.Enums;
using RythmPaperMapEditor.Models;

namespace RythmPaperMapEditor.Data
{
    public static class Data
    {
        public static readonly Dictionary<NoteType, string> NoteIcons = new Dictionary<NoteType, string>
        {
            { NoteType.NoteDefault, "NoteDefaultIcon" }
        };
    }
}