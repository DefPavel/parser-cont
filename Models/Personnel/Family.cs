namespace parser_cont.Models.Personnel;
public class Family
{
    [JsonPropertyName("fullname")]
    public string FullName { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
}
