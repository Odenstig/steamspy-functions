
namespace functions.model
{
    public class ResponseData
    {
        public int? appid { get; set; } = null!;
        public string? name { get; set; } = null!;
        public string? developer { get; set; } = null!;
        public string? publisher { get; set; } = null!;
        public string? score_rank { get; set; } = null!;
        public int? positive { get; set; } = null!;
        public int? negative { get; set; } = null!;
        public int? userscore { get; set; } = null!;
        public string? owners { get; set; } = null!;
        public int? average_forever { get; set; } = null!;
        public int? average_2weeks { get; set; } = null!;
        public int? median_forever { get; set; } = null!;
        public int? median_2weeks { get; set; } = null!;
        public string? price { get; set; } = null!;
        public string? initialprice { get; set; } = null!;
        public string? discount { get; set; } = null!;
        public int? ccu { get; set; } = null!;
    }

    public class Test
    {
        public List<ResponseData>? TestList { get; set; }
    }
}
