﻿namespace parser_cont.Models.Education;
public class Response
{
    [JsonPropertyName("added")]
    public int? Added { get; set; }

    /*[JsonPropertyName("error")]
    public List<string>? Error { get; set; }
    */

    [JsonPropertyName("successInsert")]
    public int? SuccessInsert { get; set; }
    [JsonPropertyName("errorCount")]
    public int? ErrorCount { get; set; }
    [JsonPropertyName("errorMsg")]
    public List<string>? ErrorMsg { get; set; }
}

