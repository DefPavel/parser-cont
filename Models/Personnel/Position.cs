
namespace parser_cont.Models.Personnel;

public class Position
{
    [JsonIgnore] public int Id { get; set; }
    [JsonPropertyName("name_position")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("is_ped")] public bool IsPed { get; set; }
    [JsonPropertyName("holiday")] public short holidayLimit { get; set; }
    [JsonPropertyName("oklad_budget")] public decimal? oklad_B { get; set; }
    [JsonPropertyName("oklad_nobudget")] public decimal? oklad_N { get; set; }
    [JsonPropertyName("free_budget")] public decimal? free_b { get; set; }
    [JsonPropertyName("free_nobudget")] public decimal? free_nb { get; set; }
    [JsonPropertyName("count_budget")] public decimal? count_B { get; set; }
    [JsonPropertyName("count_nobudget")] public decimal count_NB { get; set; }
    [JsonPropertyName("priority")] public short priority { get; set; }
    [JsonPropertyName("name_genitive")] public string padeg { get; set; } = string.Empty;
}

