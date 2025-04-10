using System.ComponentModel.DataAnnotations;

namespace QuizTask.Models
{
    public class CareerLevel
    {
        [Key]
        public int LevelID { get; set; }
        public string CareerName { get; set; }
        public bool isActive { get; set; }
    }
}
