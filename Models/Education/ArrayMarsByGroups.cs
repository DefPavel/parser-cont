namespace parser_cont.Models.Education;

public class ArrayMarsByGroups
{
    [JsonPropertyName("subject")]
    public string SubjectName { get; set; } = string.Empty;
    
    [JsonPropertyName("semester")]
    public string Semester { get; set; } = string.Empty;
    
    [JsonPropertyName("control")]
    public string FormControl { get; set; } = string.Empty;
    
    [JsonPropertyName("mark")]
    public int Mark { get; set; }
    
    [JsonPropertyName("mark_5")]
    public int Mark5 { get; set; }
    
    [JsonPropertyName("mark_ects")]
    public string MarkEcts { get; set; } = string.Empty;

    [JsonIgnore] public string FirstName { get; set; } = string.Empty;
    [JsonPropertyName("idStudent")] public int IdStudent { get; set; }

    public string typeControl { get; set; } = string.Empty;
}