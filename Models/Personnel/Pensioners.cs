namespace parser_cont.Models.Personnel;

public class Pensioners
{
    [JsonPropertyName("document")]
    public string Document { get; set; }
    [JsonPropertyName("date_document")]
    public string DateDocument { get; set; }

    [JsonPropertyName("type")]
    public string TypeDocument { get; set; }
}
