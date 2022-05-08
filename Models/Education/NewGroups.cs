namespace parser_cont.Models.Education
{
    public class NewGroups
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;
        [JsonPropertyName("cont_id")]
        public int IdCont { get; set; } = 0;

        [JsonPropertyName("studentArr")]
        public List<StudentOcenka>? ArrayStudents { get; set; }
    }
}