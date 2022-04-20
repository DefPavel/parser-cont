namespace parser_cont.Models;
public class ArrayStudents
{
    [JsonPropertyName("studentArr")]
    public List<Students> ArrayStudent { get; set; } = new();
}

