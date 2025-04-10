using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizTask.Models
{
    public class CandidateDetail
    {
        [Key]
        public int CandidateID { get; set; }
        [ForeignKey("IdentityUserId")]
        public string? IdentityUserId { get; set; }
        public virtual IdentityUser? IdentityUser { get; set; }
        public string Name { get; set; }
        public string? Pic { get; set; }

        [Display(Name = "Career Level")]
        public int LevelID { get; set; }
        [ForeignKey("LevelID")]
        public virtual CareerLevel? CareerLevel { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Mobile_No { get; set; }
        public string Candidate_Email { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
    }
}
