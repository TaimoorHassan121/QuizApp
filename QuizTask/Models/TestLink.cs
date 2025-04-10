using System.ComponentModel.DataAnnotations.Schema;

namespace QuizTask.Models
{
    public class TestLink
    {
        public int TestLinkId { get; set; }
        public int CandidateID { get; set; }
        [ForeignKey("CandidateID")]
        public virtual CandidateDetail? CandidateDetail { get; set; }
        public string? Candidate_TestLink { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public string? Token { get; set; }
        public int CreatedBy { get; set; }

    }
}
