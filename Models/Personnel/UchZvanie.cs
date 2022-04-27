using System.Text.Json.Serialization;

namespace parser_cont.Models.Personnel
{
    public class UchZvanie
    {
        [JsonPropertyName("id_person")] public int IdPerson { get; set; }

        [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;

        [JsonPropertyName("department")] public string Department { get; set; } = string.Empty;

        [JsonPropertyName("date_issue")] public string? DateDateIssue { get; set; } = string.Empty;

        [JsonPropertyName("document")] public string Document { get; set; } = string.Empty;

        [JsonPropertyName("place")] public string Place { get; set; } = string.Empty;

    }
}