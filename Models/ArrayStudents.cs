namespace parser_cont.Models;
public class ArrayStudents : Response
{
    [JsonPropertyName("studentArr")]
    public List<Students> ArrayStudent { get; set; } = new();
    [JsonPropertyName("specialtyArr")]
    public List<Specialtys> ArraySpecialty { get; set; } = new();
}

