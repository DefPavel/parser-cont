namespace parser_cont.Models.Personnel;

public class ChlenAcademic
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("name_academy")]
    public string AcademicalName { get; set; }

    [JsonPropertyName("name_diplom")]
    public string NumberDiplom { get; set; }

    [JsonPropertyName("date_begin")]
    public string? Date { get; set; }
}
