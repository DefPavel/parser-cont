namespace parser_cont.Models.Personnel;

public class Invalids
{
    [JsonPropertyName("document")]
    public string NameDocument { get; set; } = string.Empty;
    [JsonPropertyName("date_begin")]
    public string DateBegin { get; set; } = string.Empty;
    [JsonPropertyName("date_end")]
    public string DateEnd { get; set; } = string.Empty;
    [JsonPropertyName("for_life")]
    public bool ForLife { get; set; }
    [JsonPropertyName("group")]
    public int Group { get; set; }
}
