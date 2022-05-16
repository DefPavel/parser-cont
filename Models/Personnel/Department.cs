namespace parser_cont.Models.Personnel;
public class Department 
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonPropertyName("name_depart")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("short")]
    public string Short { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("root")]
    public string Root { get; set; } = string.Empty;

    [JsonPropertyName("name_genitive")]
    public string Padeg { get; internal set; } = string.Empty;
    [JsonPropertyName("positions")]
    public IEnumerable<Position> Positions { get; set; } = Enumerable.Empty<Position>();
}

