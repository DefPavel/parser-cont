namespace parser_cont.Models.Personnel;

    public class Medical
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("category")]
        public string Type { get; set; }
        [JsonPropertyName("date_start")]
        public string? DateBegin { get; set; }
        [JsonPropertyName("date_end")]
        public string? DateEnd { get; set; }
    }
