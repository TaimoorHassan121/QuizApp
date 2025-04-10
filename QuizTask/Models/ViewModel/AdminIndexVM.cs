namespace QuizTask.Models.ViewModel
{
    public class AdminIndexVM
    {
        public List<CandidateDetail> candidateDetails { get; set; }
        public List<TestLink> testLinks { get; set; }
        public List<QuizAttempt> quizAttempts { get; set; }
        public List<QuizQuestionAnswer> quizQuestionAnswers { get; set; }
        public List<EmailDetail> emailDetails { get; set; }

        public int EmailCount { get; set; }
        public int CandidateCount { get; set; }
        public int AttemptQuizCount { get; set; }
        public int TestLinkCount { get; set; }
    }
}
