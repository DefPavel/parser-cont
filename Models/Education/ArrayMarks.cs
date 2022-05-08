namespace parser_cont.Models.Education
{
    public class ArrayMarks : ResponceMarks
    {
        [JsonPropertyName("marksArr")]
        public List<NewGroups> Arrays { get; set; } = new();
    }
}
