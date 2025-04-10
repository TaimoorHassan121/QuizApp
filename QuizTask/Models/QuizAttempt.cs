using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizTask.Models
{
    public class QuizAttempt
    {
        [Key]
        public int QuizAtmp_ID { get; set; }
        public int CandidateID { get; set; }
        [ForeignKey("CandidateID")]
        public CandidateDetail CandidateDetail { get; set; }
        public int Quiz_Attempts { get; set; }
        public int Score { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }
}
