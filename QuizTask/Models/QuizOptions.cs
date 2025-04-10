using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizTask.Models
{
    public class QuizOptions
    {
        [Key]
        public int OptID { get; set; }
        public string Options { get; set; }
        public int OptionNo { get; set; }
        public int QuizID { get; set; }
        [ForeignKey("QuizID")]
        public virtual QuizQuestionAnswer QuestionAnswer { get; set; }
 
    }
}
