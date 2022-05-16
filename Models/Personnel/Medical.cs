namespace parser_cont.Models.Personnel;

    public class Medical
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    [JsonPropertyName("category")]
        public string Type { get; set; } = string.Empty;
    [JsonPropertyName("date_start")]
        public string? DateBegin { get; set; } 
    [JsonPropertyName("date_end")]
        public string? DateEnd { get; set; } 
}
