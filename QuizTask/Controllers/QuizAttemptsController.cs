using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizTask.Data;
using QuizTask.Models;
using QuizTask.Models.ViewModel;
using static QuizTask.Models.ViewModel.QuestionData;

namespace QuizTask.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuizAttemptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public QuizAttemptsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: QuizAttempts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QuizAttempts.Include(q => q.CandidateDetail);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QuizAttempts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizAttempt = await _context.QuizAttempts
                .Include(q => q.CandidateDetail)
                .FirstOrDefaultAsync(m => m.QuizAtmp_ID == id);
            if (quizAttempt == null)
            {
                return NotFound();
            }

            return View(quizAttempt);
        }

        [AllowAnonymous]
        public async Task<IActionResult> StartQuiz(string token)
        {
            
            QuestionData questionData = new QuestionData();

            //var handler = new JwtSecurityTokenHandler();
            //var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            //var validations = new TokenValidationParameters
            //{
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = new SymmetricSecurityKey(key),
            //    ValidateIssuer = true,
            //    ValidateAudience = false,
            //    ValidIssuer = _configuration["Jwt:Issuer"]
            //};

            try
            {
                //var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                //var candidateIds = claims.FindFirst("CandidateID")?.Value;
                var testlinkID =await _context.TestLinks.Where(l => l.Token == token).FirstOrDefaultAsync();
                var candidate = await _context.CandidateDetails.Include(a=>a.CareerLevel).FirstOrDefaultAsync(c => c.CandidateID == testlinkID.CandidateID);

                questionData.TestStatus = testlinkID.Status;
                if (testlinkID.Status.Contains("LinkExpire"))
                {
                    return View(questionData);
                }
                if (testlinkID.Status.Contains("Expire"))
                {
                    return View(questionData);
                }

                questionData.candidateDetail = candidate;

                var Careellevel = await _context.CareerLevels.Where(c => c.LevelID == candidate.LevelID).FirstOrDefaultAsync();
                var quizQuestion =await _context.QuizQuestionAnswers.Where(q=>q.LevelID== Careellevel.LevelID).ToListAsync();

                questionData.CareerLevel_Name = Careellevel.CareerName;

                foreach (var item in quizQuestion)
                {
                    QuizData quizData = new QuizData();
                    quizData.numb = item.QuizID;
                    quizData.question = item.Question;
                    quizData.answer = item.Answer;

                    var optionDb = _context.QuizOptions.Where(a => a.QuizID == item.QuizID).Select(a => a.Options).ToList();
                    quizData.options = optionDb;                  
                    questionData.quizDatas.Add(quizData);
                }

                questionData.quizDatas = questionData.quizDatas.ToList();


                if (Careellevel == null)
                {
                    return Unauthorized("Invalid Career Level");
                }

                if (candidate == null)
                {
                    return Unauthorized("Invalid token");
                }

                //Check if test is still valid
                if (testlinkID.EndDate < DateTime.UtcNow)
                {
                    return BadRequest("Test link expired");
                }

                return View(questionData);
            }
            catch
            {
                return Unauthorized("Invalid or expired token");
            }


            return View();
        }







        // GET: QuizAttempts/Create
        public IActionResult Create()
        {
            ViewData["CandidateID"] = new SelectList(_context.CandidateDetails, "CandidateID", "CandidateID");
            return View();
        }

        // POST: QuizAttempts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizAtmp_ID,CandidateID,Quiz_Attempts,Score,StartDate,EndDate,Status")] QuizAttempt quizAttempt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quizAttempt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CandidateID"] = new SelectList(_context.CandidateDetails, "CandidateID", "CandidateID", quizAttempt.CandidateID);
            return View(quizAttempt);
        }

        // GET: QuizAttempts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizAttempt = await _context.QuizAttempts.FindAsync(id);
            if (quizAttempt == null)
            {
                return NotFound();
            }
            ViewData["CandidateID"] = new SelectList(_context.CandidateDetails, "CandidateID", "CandidateID", quizAttempt.CandidateID);
            return View(quizAttempt);
        }

        // POST: QuizAttempts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizAtmp_ID,CandidateID,Quiz_Attempts,Score,StartDate,EndDate,Status")] QuizAttempt quizAttempt)
        {
            if (id != quizAttempt.QuizAtmp_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quizAttempt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizAttemptExists(quizAttempt.QuizAtmp_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CandidateID"] = new SelectList(_context.CandidateDetails, "CandidateID", "CandidateID", quizAttempt.CandidateID);
            return View(quizAttempt);
        }

        // GET: QuizAttempts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizAttempt = await _context.QuizAttempts
                .Include(q => q.CandidateDetail)
                .FirstOrDefaultAsync(m => m.QuizAtmp_ID == id);
            if (quizAttempt == null)
            {
                return NotFound();
            }

            return View(quizAttempt);
        }

        // POST: QuizAttempts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quizAttempt = await _context.QuizAttempts.FindAsync(id);
            if (quizAttempt != null)
            {
                _context.QuizAttempts.Remove(quizAttempt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizAttemptExists(int id)
        {
            return _context.QuizAttempts.Any(e => e.QuizAtmp_ID == id);
        }
    }
}
