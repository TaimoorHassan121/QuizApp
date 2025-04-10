using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizTask.Models
{
    public class EmailDetail
    {
        [Key]
        public int EmailID { get; set; }
        public int TestLinkId { get; set; }
        [ForeignKey("TestLinkId")]
        public virtual TestLink? TestLinks { get; set; }
        public string Confirmed_Email { get; set; }
        public string? Subject { get; set; }
        public string? body { get; set; }
        public DateTime Created_Date { get; set; }
        public int Created_By { get; set; }
    }
}
