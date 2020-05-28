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
        public string Image { get; set; }

        [JsonProperty("votes")]
        public float Votes { get; set; }

        public int Percent { get; set; }
    }
}