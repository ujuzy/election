﻿using Android.Graphics;
using Android.Widget;
using Newtonsoft.Json;

namespace Election
{
    public class CandidateData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("secondname")]
        public string SecondName { get; set; }

        [JsonProperty("thirdname")]
        public string ThirdName { get; set; }

        [JsonProperty("party")]
        public string Party { get; set; }

        [JsonProperty("description")]
        public string Descriptions { get; set; }

        [JsonProperty("web")]
        public string Web { get; set; }

        [JsonProperty("image")]
        public string ImageUrl { get; set; }

        [JsonProperty("votes")]
        public float Votes { get; set; }

        public int Percent { get; set; }

        //0 - голоса нет, 1 - голос есть, 2 - предыдущий кандидат
        public int IsVoiceSent { get; set; }

        public Bitmap Image { get; set; }
    }
}