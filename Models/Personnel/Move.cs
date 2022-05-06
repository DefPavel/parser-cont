namespace parser_cont.Models.Personnel;
public class Move
{
    [JsonPropertyName("date_crt")]
    public string dateInsert { get; internal set; }

    [JsonPropertyName("count_budget")]
    public decimal count_budget { get; internal set; }
    [JsonPropertyName("count_nobudget")]
    public decimal count_nobudget { get; internal set; }

    [JsonPropertyName("order")]
    public string order { get; internal set; }

    [JsonPropertyName("date_order")]
    public string DateOrder { get; internal set; }

    [JsonPropertyName("type_order")]
    public string typeOrder { get; internal set; }
    [JsonPropertyName("name_dep")]
    public string name_dep { get; internal set; }
    [JsonPropertyName("position")]
    public string position { get; internal set; }
    [JsonPropertyName("is_main")]
    public bool isMain { get; internal set; }
    
    [JsonPropertyName("is_ped")]
    public bool isPed { get; internal set; }
    [JsonPropertyName("contract")]
    public string Contract { get; internal set; }
    [JsonPropertyName("date_start")]
    public string dateBegin { get; internal set; }
    [JsonPropertyName("date_end")]
    public string dateEnd { get; internal set; }
    [JsonPropertyName("date_drop")]
    public string dateDrop { get; internal set; }

    [JsonPropertyName("day_holiday")]
    public int dayVacation { get; internal set; }

    [JsonPropertyName("pers_id")]
    public int PersonId { get; internal set; }
    [JsonPropertyName("order_drop")]
    public string? orderDrop { get; internal set; }
}
