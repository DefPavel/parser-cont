namespace parser_cont.Models.Personnel;

    public class Persons
    {
        [JsonPropertyName("pers_id")]
        public int persId { get; internal set; }
        //[JsonIgnore]
        public string photo { get; internal set; } = string.Empty;

        [JsonPropertyName("date_to_working")]
        public string DateToWorking { get; internal set; } = string.Empty;

        [JsonPropertyName("firstname")]
        public string FirstName { get; internal set; } = string.Empty;
    [JsonPropertyName("name")]
        public string Name { get; internal set; } = string.Empty;
    [JsonPropertyName("lastname")]
        public string LastName { get; internal set; } = string.Empty;
    [JsonPropertyName("gender")]
        public string Gender { get; internal set; } = string.Empty;
    [JsonPropertyName("birthday")]
        public string DateBirthay { get; internal set; } = string.Empty;
    [JsonPropertyName("identification_code")]
        public string Code { get; internal set; } = string.Empty;
    [JsonPropertyName("phone_ua")]
        public string PhoneUA { get; internal set; } = string.Empty;
    [JsonPropertyName("phone_lug")]
        public string PhoneLG { get; internal set; } = string.Empty;
    [JsonPropertyName("adress")]
        public string Adress { get; internal set; } = string.Empty;
    [JsonPropertyName("type_passport")]
        public string TypePassport { get; internal set; } = string.Empty;
    [JsonPropertyName("searial_passport")]
        public string SerialPassport { get; internal set; } = string.Empty;
    [JsonPropertyName("number_passport")]
        public string NumberPassport { get; internal set; } = string.Empty;
    [JsonPropertyName("date_passport")]
        public string DatePassport { get; internal set; } = string.Empty;
    [JsonPropertyName("organization_passport")]
        public string OrganiztionPassport { get; internal set; } = string.Empty;
    [JsonPropertyName("russion_passport")]
        public bool IsRusPassport { get; internal set; } 
    [JsonPropertyName("marriage")]
        public bool IsMarriage { get; internal set; }
    [JsonPropertyName("is_student")]
        public bool IsStudent { get; internal set; } 
    [JsonPropertyName("is_graduate")]
        public bool IsAspirant { get; internal set; }
        [JsonPropertyName("is_doctor")]
        public bool IsDoctor { get; internal set; }
        [JsonPropertyName("is_snils")]
        public bool IsSnils { get; internal set; }
        [JsonPropertyName("is_responsible")]
        public bool IsResponsible { get; internal set; }
        [JsonPropertyName("is_single_mother")]
        public bool IsSingleMother { get; internal set; }
        [JsonPropertyName("is_two_child_mother")]
        public bool IsTwoChildMother { get; internal set; }
        [JsonPropertyName("is_previos_convition")]
        public bool IsPreviosConvition { get; internal set; }
        [JsonPropertyName("name_dative")]
        public string Padeg { get; internal set; } = string.Empty;

    [JsonPropertyName("positions")]
        public IEnumerable<PersonPosition> ArrayPositions { get; internal set; } = Enumerable.Empty<PersonPosition>();

        [JsonPropertyName("changeSurname")]
        public IEnumerable<ChangeSurname> ArraySurname { get; internal set; } = Enumerable.Empty<ChangeSurname>();
    [JsonPropertyName("educations")]
        public IEnumerable<Education> ArraEducation { get; internal set; } = Enumerable.Empty<Education>();
    [JsonPropertyName("employmentHistory")]
        public IEnumerable<HistoryBook> ArrayHistoryBook { get; internal set; } = Enumerable.Empty<HistoryBook>();

        [JsonPropertyName("personMove")]
        public IEnumerable<Move> ArrayMove { get; internal set; } = Enumerable.Empty<Move>();

        [JsonPropertyName("familyPerson")]
        public IEnumerable<Family> ArrayFamily { get; internal set; } = Enumerable.Empty<Family>();

    [JsonPropertyName("pensioner")]
        public IEnumerable<Pensioners> ArrayPensioners { get; internal set; } = Enumerable.Empty<Pensioners>();
    [JsonPropertyName("invalid")]
        public IEnumerable<Invalids> ArrayInvalids { get; internal set; } = Enumerable.Empty<Invalids>();

        [JsonPropertyName("medicals")]
        public IEnumerable<Medical> ArrayMedical { get; internal set; } = Enumerable.Empty<Medical>();
    [JsonPropertyName("memberAcademic")]
        public IEnumerable<ChlenAcademic> ArrayAcademics { get; internal set; } = Enumerable.Empty<ChlenAcademic>();
}
