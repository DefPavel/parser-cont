namespace parser_cont.Models.Personnel;

public class ChlenAcademic
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("name_academy")]
    public string AcademicalName { get; set; } = string.Empty;

    [JsonPropertyName("name_diplom")]
    public string NumberDiplom { get; set; } = string.Empty;

    [JsonPropertyName("date_begin")]
    public string? Date { get; set; } = string.Empty;
}
