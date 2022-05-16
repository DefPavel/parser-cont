namespace parser_cont.Models.Personnel;

public class ScientificDegree
{
    [JsonPropertyName("id_person")]
    public int PersonId { get; set; } // id Персоны
    [JsonPropertyName("document")]
    public string NumberDocument { get; set; } = string.Empty;  // номер документа
    [JsonPropertyName("scientific_branch")]
    public string ScientificBranch { get; set; } = string.Empty; // Научная отрасл
    [JsonPropertyName("scientific_specialty")] public string ScientificSpecialty { get; set; } = string.Empty; // Научная специалность
    [JsonPropertyName("dissertation")] public string Dissertation { get; set; } = string.Empty; // Тема диссертации
    [JsonPropertyName("date_of_issue")] public string? DateOfIssue { get; set; } // Дата утверждения
    [JsonPropertyName("place")] public string Place { get; set; } = string.Empty; // Место присвоения
    [JsonPropertyName("city")] public string City { get; set; } = string.Empty; // Город присвоения
    [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;// Примечание
    [JsonPropertyName("job_after")] public string JobAfter { get; set; } = string.Empty; // Должность где работал после получения диплома
    [JsonPropertyName("count_scientific")] public int CountScientific { get; set; } // Количество научных
    [JsonPropertyName("request_scientific")] public string RequestScientific { get; set; } = string.Empty; // Количество заявок
    [JsonPropertyName("chage_scientific")] public string Change { get; set; } = string.Empty; // Изменения
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty; // Тип Степени
}
