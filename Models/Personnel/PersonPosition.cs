namespace parser_cont.Models.Personnel;

public class PersonPosition
{
    [JsonPropertyName("position")]
    public string Position { get; set; }

    [JsonPropertyName("department")]
    public string Department { get; set; }
    [JsonPropertyName("order")]
    public string Order { get; set; }
    [JsonPropertyName("date_order")]
    public string DateOrder { get; set; }
    [JsonPropertyName("type_order")]
    public string TypeOrder { get; set; }
    [JsonPropertyName("contract")]
    public string Contract { get; set; }
    [JsonPropertyName("place")]
    public string Place { get; set; }
    [JsonPropertyName("is_main")]
    public bool IsMain { get; set; }
    [JsonPropertyName("is_ped")]
    public bool IsPed { get; set; }

    [JsonPropertyName("count_budget")]
    public decimal CountBudget { get; set; }
    [JsonPropertyName("count_nobudget")]
    public decimal CountNoBudget { get; set; }
    [JsonPropertyName("is_pluralism_inner")]
    public bool IsPluralismInner { get; set; } // Совместитель
    [JsonPropertyName("is_pluralism_oter")]
    public bool IsPluralismOter { get; set; } // Внешний совместитель
    [JsonPropertyName("data_start_contract")]
    public string? DateStartContract { get; set; }
    [JsonPropertyName("date_end_contract")]
    public string? DateEndContract { get; set; }
    [JsonPropertyName("date_drop")]
    public string? DateDrop { get; set; }
    [JsonPropertyName("position_drop")]
    public string? PositionDrop { get; set; }
}
