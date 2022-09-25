namespace parser_cont.Models.Education;
public class ArrayStudents 
{
    [JsonPropertyName("studentArr")]
    public IEnumerable<Students> ArrayStudent { get; init; } = Enumerable.Empty<Students>();
    
    [JsonIgnore]
    //[JsonPropertyName("specialtyArr")]
    public IEnumerable<Specialtys> ArraySpecialty { get; init; } = Enumerable.Empty<Specialtys>();
}

