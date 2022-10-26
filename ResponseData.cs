
using System.Text.Json.Serialization;

namespace functions.model
{
    public class ResponseData
    {
        [JsonPropertyName("appid")]
        public int Appid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("developer")]
        public string Developer { get; set; }

        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }

        [JsonPropertyName("score_rank")]
        public string ScoreRank { get; set; }

        [JsonPropertyName("positive")]
        public int Positive { get; set; }

        [JsonPropertyName("negative")]
        public int Negative { get; set; }

        [JsonPropertyName("userscore")]
        public int Userscore { get; set; }

        [JsonPropertyName("owners")]
        public string Owners { get; set; }

        [JsonPropertyName("average_forever")]
        public int AverageForever { get; set; }

        [JsonPropertyName("average_2weeks")]
        public int Average2weeks { get; set; }

        [JsonPropertyName("median_forever")]
        public int MedianForever { get; set; }

        [JsonPropertyName("median_2weeks")]
        public int Median2weeks { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("initialprice")]
        public string Initialprice { get; set; }

        [JsonPropertyName("discount")]
        public string Discount { get; set; }

        [JsonPropertyName("ccu")]
        public int Ccu { get; set; }
    }
}
