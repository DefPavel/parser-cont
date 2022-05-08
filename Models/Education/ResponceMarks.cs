namespace parser_cont.Models.Education
{
    public class ResponceMarks
    {
        [JsonPropertyName("error")]
        public Error? Error { get; set; }

        //[JsonPropertyName("addedSubjects")]
        //public AddedSubjects? AddedSubjects { get; set; }

        [JsonPropertyName("successCount")]
        public int SuccessCount { get; set; }
    }

    public class AddedSubjects
    {
        public int count { get; set; }
        public List<string>? arr { get; set; }
    }

    public class Error
    {
        public List<string>? errorMsg { get; set; }
        public int errorCount { get; set; }
    }
}
