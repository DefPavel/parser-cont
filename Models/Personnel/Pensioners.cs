namespace parser_cont.Models.Personnel;

public class Pensioners
{
    [JsonPropertyName("document")]
    public string Document { get; set; } = string.Empty;
    [JsonPropertyName("date_document")]
    public string DateDocument { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string TypeDocument { get; set; } = string.Empty;
}
