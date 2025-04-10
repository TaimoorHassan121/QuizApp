namespace QuizTask.Models.ViewModel
{
    public class QuestionData
    {
        public string CareerLevel_Name { get; set; }
        public string TestStatus { get; set; }
        public CandidateDetail candidateDetail { get; set; }
        public List<QuizData> quizDatas { get; set; } = new List<QuizData>();
        public class QuizData
        {
            public int numb { get; set; }
            public string question { get; set; }
            public string answer { get; set; }
            public List<string> options { get; set; }
        }
    }
}
