namespace parser_cont.Models;
public class StudentOcenka
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("uchPlan")]
    public List<Plan> Plans { get; set; } = new();
}

