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

    public string RecordBook { get; internal set; } = string.Empty;
    public string IsBudget { get; internal set; } = string.Empty;

    public string orderName { get; internal set; } = string.Empty;
    public string? orderDate { get; internal set; }

    public IEnumerable<Orders> Orders { get; set; } = Enumerable.Empty<Orders>();
    // Ссылка на группы
    [JsonIgnore]
    public IEnumerable<Groups> Groups { get; set; } = Enumerable.Empty<Groups>();
    [JsonIgnore]
    public IEnumerable<OrganizationEducation> OrganizationEducation { get; internal set; } = Enumerable.Empty<OrganizationEducation>();
    // Ссылка на Родство
    public IEnumerable<Relatives> Relatives { get; set; } = Enumerable.Empty<Relatives>();
}

