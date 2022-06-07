namespace parser_cont.Models.Personnel;
public class Family
{
    [JsonPropertyName("fullname")]
    public string FullName { get; set; } = string.Empty;
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("birthday")]
    public string? Birthday { get; set; }
}
