namespace parser_cont.Models.Education
{
    public class ResponceMarks
    {
        public ResultMarks? resultMarks { get; set; }
        public ResultSubject? resultSubject { get; set; }
    }

    public class ResultMarks
    {
        public int added { get; set; }
        public List<string>? error { get; set; }
    }

    public class ResultSubject
    {
        public int added { get; set; }
        //public List<object> arr { get; set; }
    }
}
