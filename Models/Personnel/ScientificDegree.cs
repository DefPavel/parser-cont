using System.Text.Json.Serialization;

namespace parser_cont.Models.Personnel
{
    public class ScientificDegree
    {

        [JsonPropertyName("id_person")]
        public int PersonId { get; set; } // id Персоны
        [JsonPropertyName("document")]
        public string NumberDocument { get; set; }  // номер документа
        [JsonPropertyName("scientific_branch")]
        public string ScientificBranch { get; set; } // Научная отрасл
        [JsonPropertyName("scientific_specialty")]
        public string ScientificSpecialty { get; set; } // Научная специалность
        [JsonPropertyName("dissertation")]
        public string Dissertation { get; set; } // Тема диссертации
        [JsonPropertyName("date_of_issue")]
        public string? DateOfIssue { get; set; } // Дата утверждения
        [JsonPropertyName("place")]
        public string Place { get; set; } // Место присвоения
        [JsonPropertyName("city")]
        public string City { get; set; } // Город присвоения
        [JsonPropertyName("description")]
        public string Description { get; set; }// Примечание
        [JsonPropertyName("job_after")]
        public string JobAfter { get; set; } // Должность где работал после получения диплома
        [JsonPropertyName("count_scientific")]
        public int CountScientific { get; set; } // Количество научных
        [JsonPropertyName("request_scientific")]
        public string RequestScientific { get; set; } // Количество заявок
        [JsonPropertyName("chage_scientific")]
        public string Change { get; set; } // Изменения
        [JsonPropertyName("type")]
        public string Type { get; set; } // Тип Степени
    }
}