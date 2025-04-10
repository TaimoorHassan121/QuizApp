using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizTask.Models
{
    public class AdminUser
    {
        [Key]
        public int UserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public string? IdentityUserId { get; set; }
        public virtual IdentityUser? IdentityUser { get; set; }
        public int User_Reg_No { get; set; }
        public string? User_Name { get; set; }
        [ForeignKey("IdentityRoleId")]
        public string? Role { get; set; }
        public virtual IdentityRole? IdentityRole { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Mobile_No { get; set; }
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? User_Email { get; set; }
        [DataType(DataType.Password)]
        public string? User_Passward { get; set; }
        public bool User_Status { get; set; } = true;
        public DateTime User_Date { get; set; }
    }
}
