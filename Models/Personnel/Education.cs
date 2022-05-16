namespace parser_cont.Models.Personnel;
public class Education
{
    public string institution { get; internal set; } = string.Empty;
    public string type { get; internal set; } = string.Empty;
    public string specialty { get; internal set; } = string.Empty;
    public string qualification { get; internal set; } = string.Empty;
    public string? date_issue { get; internal set; } = string.Empty;
    public string name_diplom { get; internal set; } = string.Empty;
    public bool is_actual { get; internal set; } 
}
