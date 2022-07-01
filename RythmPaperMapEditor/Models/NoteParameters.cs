using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RythmPaperMapEditor.Enums;

namespace RythmPaperMapEditor.Models
{
    public class NoteParameters
    {
        private static Random _random = new Random();
     
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("sex")]
        public Sex Sex { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("country")]
        public Country Country { get; }

        public NoteParameters(Sex sex, Country country)
        {
            Sex = sex;
            Country = country;
        }

        public static NoteParameters Random()
        {
            var randomSex = (Sex) _random.Next(Enum.GetNames(typeof(Sex)).Length);
            var randomCountry = (Country)_random.Next(Enum.GetNames(typeof(Country)).Length);

            return new NoteParameters(randomSex, randomCountry);
        } 
    }
}