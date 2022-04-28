namespace parser_cont.Models.Education;
public class ArrayStudents : Response
{
    [JsonPropertyName("studentArr")]
    public List<Students> ArrayStudent { get; init; } = new();
    [JsonPropertyName("specialtyArr")]
    public List<Specialtys> ArraySpecialty { get; init; } = new();
}

