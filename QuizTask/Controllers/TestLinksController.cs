using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizTask.Data;
using QuizTask.Models;
using QuizTask.Services;

namespace QuizTask.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TestLinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public TestLinksController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: TestLinks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TestLinks.Include(t => t.CandidateDetail);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TestLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testLink = await _context.TestLinks
                .Include(t => t.CandidateDetail)
                .FirstOrDefaultAsync(m => m.TestLinkId == id);
            if (testLink == null)
            {
                return NotFound();
            }

            return View(testLink);
        }

        // GET: TestLinks/Create
        public IActionResult Create()
        {
            ViewData["CandidateID"] = new SelectList(_context.CandidateDetails.Where(c=>c.Status.Contains("Pending")), "CandidateID", "Name");
            return View();
        }

        // POST: TestLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestLinkId,CandidateID,Candidate_TestLink,StartDate,EndDate,Status,Token,CreatedBy")] TestLink testLink)
        {
            string userId = HttpContext.Session.GetString("UserId");

            var candidateId = testLink.CandidateID;
            var candidate = await _context.CandidateDetails.FindAsync(candidateId);
            if (candidate == null)
            {
                return NotFound("Candidate not found");
            }
            var tokenService = new TokenService(_configuration);
            var token = tokenService.GenerateToken(candidate);
            testLink.Token = token;
            var link = $"https://localhost:7283/QuizAttempts/StartQuiz?token={token}";
            testLink.Candidate_TestLink = link;
            testLink.CreatedBy = Convert.ToInt32(userId);
            if (ModelState.IsValid)
            {
                _context.Add(testLink);
                await _context.SaveChangesAsync();

                candidate.Status = "LinkCreate";
                _context.Update(candidate);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            ViewData["CandidateID"] = new SelectList(_context.CandidateDetails.Where(c => c.Status.Contains("Pending")), "CandidateID", "Name");
            return View(testLink);
        }

        // GET: TestLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testLink = await _context.TestLinks.FindAsync(id);
            if (testLink == null)
            {
                return NotFound();
            }
            ViewData["CandidateID"] = new SelectList(_context.CandidateDetails, "CandidateID", "Name", testLink.CandidateID);
            return View(testLink);
        }

        // POST: TestLinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TestLinkId,CandidateID,Candidate_TestLink,StartDate,EndDate,Status,Token,CreatedBy")] TestLink testLink)
        {
            if (id != testLink.TestLinkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestLinkExists(testLink.TestLinkId))
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
            ViewData["CandidateID"] = new SelectList(_context.CandidateDetails, "CandidateID", "Name", testLink.CandidateID);
            return View(testLink);
        }

        // GET: TestLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testLink = await _context.TestLinks
                .Include(t => t.CandidateDetail)
                .FirstOrDefaultAsync(m => m.TestLinkId == id);
            if (testLink == null)
            {
                return NotFound();
            }

            return View(testLink);
        }

        // POST: TestLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testLink = await _context.TestLinks.FindAsync(id);
            if (testLink != null)
            {
                _context.TestLinks.Remove(testLink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestLinkExists(int id)
        {
            return _context.TestLinks.Any(e => e.TestLinkId == id);
        }
    }
}
