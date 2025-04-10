using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizTask.Data;
using QuizTask.Models;
using QuizTask.Services;

namespace QuizTask.Controllers
{
    public class QuizLinkController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public QuizLinkController(ApplicationDbContext context, IConfiguration configuration)
        { 
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CandidateLinkDetail()
        {
            var linkDetail = _context.TestLinks.ToListAsync();
            return View(linkDetail);
        }

        [HttpPost("generate-link")]
        public async Task<IActionResult> GenerateQuizLink(int candidateId,TestLink testLink)
        {
            var candidate = await _context.CandidateDetails.FindAsync(candidateId);
            if (candidate == null)
            {
                return NotFound("Candidate not found");
            }

            // Generate Token
            var tokenService = new TokenService(_configuration);
            var token = tokenService.GenerateToken(candidate);

            // Save token in DB
            testLink.Token = token;
            var link = $"https://yourquizapp.com/startQuiz?token={token}";
            testLink.Candidate_TestLink = link;
            _context.CandidateDetails.Add(candidate);
            await _context.SaveChangesAsync();


            return Ok(new { Link = link });
        }
    }
}
