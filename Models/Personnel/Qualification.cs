
namespace parser_cont.Models.Personnel;

public class Qualification
{
    [JsonPropertyName("id_person")] public int IdPerson { get; set; }
    [JsonPropertyName("name_course")] public string CourseName { get; set; } = string.Empty;
    [JsonPropertyName("date_start")] public string DateBegin { get; set; } = string.Empty;
    [JsonPropertyName("date_end")] public string DateEnd { get; set; } = string.Empty;
    [JsonPropertyName("place")] public string Place { get; set; } = string.Empty;
    [JsonPropertyName("certificate")] public string NumberCertificate { get; set; } = string.Empty;
    [JsonPropertyName("date_issue")] public string DateIssue { get; set; } = string.Empty;
}
