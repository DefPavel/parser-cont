namespace parser_cont.Models.Education;

public class NewArrayMarks
{
    [JsonPropertyName("idStudent")]
    public int IdStudent { get; set; }
    
    [JsonPropertyName("marksArr")]
    public IEnumerable<NewMarks> ArrayMarks { get; set; } = Enumerable.Empty<NewMarks>();
}