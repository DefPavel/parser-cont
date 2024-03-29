﻿namespace parser_cont.Models.Education;

public class NewMarks
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
    [JsonIgnore] public string MiddleName { get; set; } = string.Empty;
    [JsonIgnore] public string LastName { get; set; } = string.Empty;
    
    [JsonIgnore] public string NickFacult { get; set; } = string.Empty;
    [JsonIgnore] public string NickLevel { get; set; } = string.Empty;
    [JsonIgnore] public int Course { get; set; }
    [JsonIgnore] public int IdStudent { get; set; }

    public string typeControl { get; set; } = string.Empty;
}