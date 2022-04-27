namespace parser_cont.Models.Personnel;
public class HistoryBook
{
    [JsonPropertyName("number_record")]
    public int numberRecord { get; internal set; }
    [JsonPropertyName("created_at")]
    public string dateInsert { get; internal set; }
    public string information { get; internal set; }
    [JsonPropertyName("is_over")]
    public bool isOver { get; internal set; }
    [JsonPropertyName("is_pedagogical")]
    public bool isPedagogical { get; internal set; }
    [JsonPropertyName("is_science")]
    public bool isScience { get; internal set; }
    [JsonPropertyName("is_univer")]
    public bool isUniver { get; internal set; }
    [JsonPropertyName("is_library")]
    public bool isLibrary { get; internal set; }
    [JsonPropertyName("is_museum")]
    public bool isMuseum { get; internal set; }
    [JsonPropertyName("is_medical")]
    public bool isMedical { get; internal set; }
    [JsonPropertyName("order_name")]
    public string orderName { get; internal set; }
}
