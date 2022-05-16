namespace parser_cont.Models.Personnel;
public class Documents
{
    [JsonPropertyName("id_person")]
    public int IdPers { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("document")]
    public byte[]? Document { get; set; }
}
