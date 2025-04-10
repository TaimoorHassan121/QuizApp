using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.FileIO;

namespace QuizTask.Models
{
    public class QuizQuestionAnswer
    {
        [Key]
        public int QuizID { get; set; }
        public int QuizQno { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int LevelID { get; set; }
        [ForeignKey("LevelID")]
        public virtual CareerLevel? CareerLevel { get; set; }
        public bool QuizStatus { get; set; }
        public DateTime QuizDate { get; set; }
        public virtual List<QuizOptions> QOptions { get; set; } = new List<QuizOptions>();
        //public virtual ICollection<QuizOptions>? QOptions { get; set; }
    }
}
