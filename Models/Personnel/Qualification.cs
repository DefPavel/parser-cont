using System.Text.Json.Serialization;

namespace parser_cont.Models.Personnel
{
    public class Qualification
    {
        [JsonPropertyName("id_person")]
        public int IdPerson { get; set; }
        [JsonPropertyName("name_course")]
        public string CourseName { get; set; }
        [JsonPropertyName("date_start")]
        public string DateBegin { get; set; }
        [JsonPropertyName("date_end")]
        public string DateEnd { get; set; }
        [JsonPropertyName("place")]
        public string Place { get; set; }
        [JsonPropertyName("certificate")]
        public string NumberCertificate { get; set; }
        [JsonPropertyName("date_issue")]
        public string DateIssue { get; set; }
    }
}