using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizTask.Models;

namespace QuizTask.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public DbSet<CareerLevel> CareerLevels { get; set; }
        public DbSet<QuizQuestionAnswer> QuizQuestionAnswers { get; set; }
        public DbSet<QuizOptions> QuizOptions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<QuizLog> QuizLogs { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<CandidateDetail> CandidateDetails { get; set; }
        public DbSet<TestLink> TestLinks { get; set; }
        public DbSet<EmailDetail> EmailDetails { get; set; }



    }
}
