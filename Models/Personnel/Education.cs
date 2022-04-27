namespace parser_cont.Models.Personnel;
public class Education
{
    public string institution { get; internal set; }
    public string type { get; internal set; }
    public string specialty { get; internal set; }
    public string qualification { get; internal set; }
    public string? date_issue { get; internal set; }
    public string name_diplom { get; internal set; }
    public bool is_actual { get; internal set; }
}
