namespace parser_cont.Models.Personnel;

public class GlobalArray : ResponceSync
{
    // Синхронизация для отделов и должностей
    public IEnumerable<Department> ArrayDepartments { get; set; } = Enumerable.Empty<Department>();
    // Основная информация о человеке 
    public IEnumerable<Persons> ArrayPersons { get; set; } = Enumerable.Empty<Persons>();
    // Информация об отпусках
    public IEnumerable<Vacations> ArrayVacation { get; set; } = Enumerable.Empty<Vacations>();
    // Информация о награждениях
    public IEnumerable<Rewarding> ArrayRewarding { get; set; } = Enumerable.Empty<Rewarding>();
    // Парсер сканов документов
    public IEnumerable<Documents> ArrayDocuments { get; set; } = Enumerable.Empty<Documents>();
    // Повыщение квалификации
    public IEnumerable<Qualification> ArrayQualification { get; set; } = Enumerable.Empty<Qualification>();
    // Ученное звание
    public IEnumerable<UchZvanie> ArrayAcademicTitle { get; set; } = Enumerable.Empty<UchZvanie>();
    // Служебные перемещения
    public IEnumerable<Move> ArrayMove { get; set; } = Enumerable.Empty<Move>();
    // Научная степень
    public IEnumerable<ScientificDegree> ArrayDegrees { get; set; } = Enumerable.Empty<ScientificDegree>();
    // Парсер фотографий 3x4
    public IEnumerable<Image> ArrayImage { get; set; } = Enumerable.Empty<Image>();
}

public class ResponceSync
{
    [JsonPropertyName("success")]
    public int Success { get; set; } = 0;
    [JsonPropertyName("updated")]
    public int Updated { get; set; } = 0;
    [JsonPropertyName("error")]
    public int Failed { get; set; } = 0;
    [JsonPropertyName("details")]
    public string[] Details = Array.Empty<string>();
}

