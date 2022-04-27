namespace parser_cont.Models.Education;
public class Students
{
    public int IdStudent { get; internal set; } = 0;
    public string FirstName { get; internal set; } = string.Empty;
    public string MiddleName { get; internal set; } = string.Empty;
    public string LastName { get; internal set; } = string.Empty;
    public string Birthday { get; internal set; } = string.Empty;
    public string Gender { get; internal set; } = string.Empty;
    public string Citizen { get; internal set; } = string.Empty;
    public string BirthPlace { get; set; } = string.Empty;
    public string Code { get; internal set; } = string.Empty;
    public string FirstPhone { get; internal set; } = string.Empty;
    public string SecondPhone { get; internal set; } = string.Empty;
    public string ThirdPhone { get; internal set; } = string.Empty;
    public string AdressRegistration { get; internal set; } = string.Empty;
    public string AdressActual { get; internal set; } = string.Empty;
    public string SerialPassport { get; internal set; } = string.Empty;
    public string NumberPassport { get; internal set; } = string.Empty;
    public string OrganizationPassport { get; internal set; } = string.Empty;
    public string DatePassport { get; internal set; } = new DateTime(1900,1,1).ToShortDateString();
    public string TypeDocument { get; internal set; } = string.Empty;
    public bool IsHostel { get; internal set; } = false;
    public bool NeedHostel { get; internal set; } = false;

    // Ссылка на группы
    public List<Groups> Groups { get; set; } = new();
    public List<OrganizationEducation> OrganizationEducation { get; internal set; } = new();
    // Ссылка на Родство
    public List<Relatives> Relatives { get; set; } = new();
}

