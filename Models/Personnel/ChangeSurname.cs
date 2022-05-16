namespace parser_cont.Models.Personnel;

public class ChangeSurname
{
    [JsonPropertyName("old_surname")]
    public string OldSurname { get; internal set; } = string.Empty;
    [JsonPropertyName("order")]
    public string Order { get; internal set; } = string.Empty ;
    [JsonPropertyName("type_order")]
    public string TypeOrder { get; internal set; } = string.Empty;
    [JsonPropertyName("date_order")]
    public string DateOrder { get; internal set; } = string.Empty;
}
