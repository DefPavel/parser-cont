namespace parser_cont.Models.Education;

public class Plan
{
    [JsonPropertyName("subjectName")]
    public string SubjectName { get; set; } = string.Empty;
    [JsonPropertyName("semester")]
    public string Semester { get; set; } = string.Empty;
    [JsonPropertyName("ball_100")]
    public int Ball_100 { get; set; }
    [JsonPropertyName("ball_5")]
    public string Ball_5 { get; set; } = string.Empty;

    [JsonPropertyName("ball_ECTS")]
    public string Ball_ECTS { get; set; } = string.Empty;

    [JsonPropertyName("formControl")]
    public string FormControl { get; set; } = string.Empty;
}

