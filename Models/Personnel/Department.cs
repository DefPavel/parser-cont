namespace parser_cont.Models.Personnel;
public class Department 
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonPropertyName("name_depart")]
    public string Name { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("short")]
    public string Short { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("root")]
    public string Root { get; set; }

    [JsonPropertyName("name_genitive")]
    public string Padeg { get; internal set; }
    [JsonPropertyName("positions")]
    public IEnumerable<Position> Positions { get; set; }
}

