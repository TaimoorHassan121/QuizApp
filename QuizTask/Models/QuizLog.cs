using System.ComponentModel.DataAnnotations;

namespace QuizTask.Models
{
    public class QuizLog
    {
        [Key]
        public int QuizLogId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public string CareelLevel { get; set; }
        public string? option1 { get; set; }
        public string? option2 { get; set; }
        public string? option3 { get; set; }
        public string? option4 { get; set; }
        public string? option5 { get; set; }
        public bool QuizStatus { get; set; }
        public DateTime Quiz_AddDate { get; set; }
        public DateTime Quiz_UpdateDate { get; set; }
    }
}
