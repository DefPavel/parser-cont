namespace parser_cont.Models.Education;
public class Groups
{
    public int IdGroup { get; internal set; } = 0;
    public string NameGroup { get; internal set; } = string.Empty;
    public int Course { get; internal set; } = 1;
    public string YearStart { get; internal set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Form { get; internal set; } = string.Empty;
    public string Faculty { get; internal set; } = string.Empty;
    public string Basis { get; internal set; } = string.Empty;
    public string RecordBook { get; internal set; } = string.Empty;

    public List<Spesialty> Specialty { get; set; } = new();
}

