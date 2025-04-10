using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using QuizTask.Data;
using QuizTask.Models;

namespace QuizTask.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CandidateDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public CandidateDetailsController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<RegisterModel> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        // GET: CandidateDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CandidateDetails.Include(c => c.CareerLevel).Include(c => c.IdentityUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CandidateDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateDetail = await _context.CandidateDetails
                .Include(c => c.CareerLevel)
                .Include(c => c.IdentityUser)
                .FirstOrDefaultAsync(m => m.CandidateID == id);
            if (candidateDetail == null)
            {
                return NotFound();
            }

            return View(candidateDetail);
        }

        // GET: CandidateDetails/Create
        public IActionResult Create()
        {
            ViewData["LevelID"] = new SelectList(_context.CareerLevels, "LevelID", "CareerName");
            return View();
        }

        // POST: CandidateDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateID,IdentityUserId,Name,Pic,LevelID,Mobile_No,Candidate_Email,Gender,IsActive,Status")] CandidateDetail candidateDetail, IFormFile profilepic)
        {
            var user = new IdentityUser { UserName = candidateDetail.Candidate_Email, Email = candidateDetail.Candidate_Email, PhoneNumber = candidateDetail.Mobile_No, EmailConfirmed = true };
            candidateDetail.IdentityUser = user;
            candidateDetail.IdentityUserId = candidateDetail.IdentityUser.Id;

            if (profilepic != null)
            {
                var image = ContentDispositionHeaderValue.Parse(profilepic.ContentDisposition).FileName.Trim();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", profilepic.FileName);
                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                {
                    profilepic.CopyTo(stream);
                }
                candidateDetail.Pic = profilepic.FileName;
            }
            if (ModelState.IsValid)
            {              
                _context.Add(candidateDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LevelID"] = new SelectList(_context.CareerLevels, "LevelID", "CareerName", candidateDetail.LevelID);
            return View(candidateDetail);
        }

        // GET: CandidateDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateDetail = await _context.CandidateDetails.FindAsync(id);
            if (candidateDetail == null)
            {
                return NotFound();
            }
            ViewData["LevelID"] = new SelectList(_context.CareerLevels, "LevelID", "CareerName", candidateDetail.LevelID);
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", candidateDetail.IdentityUserId);
            return View(candidateDetail);
        }

        // POST: CandidateDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateID,IdentityUserId,Name,Pic,LevelID,Mobile_No,Candidate_Email,Gender,IsActive")] CandidateDetail candidateDetail)
        {
            if (id != candidateDetail.CandidateID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidateDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateDetailExists(candidateDetail.CandidateID))
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
            ViewData["LevelID"] = new SelectList(_context.CareerLevels, "LevelID", "CareerName", candidateDetail.LevelID);
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", candidateDetail.IdentityUserId);
            return View(candidateDetail);
        }

        // GET: CandidateDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateDetail = await _context.CandidateDetails
                .Include(c => c.CareerLevel)
                .Include(c => c.IdentityUser)
                .FirstOrDefaultAsync(m => m.CandidateID == id);
            if (candidateDetail == null)
            {
                return NotFound();
            }

            return View(candidateDetail);
        }

        // POST: CandidateDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidateDetail = await _context.CandidateDetails.FindAsync(id);
            if (candidateDetail != null)
            {
                _context.CandidateDetails.Remove(candidateDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateDetailExists(int id)
        {
            return _context.CandidateDetails.Any(e => e.CandidateID == id);
        }
    }
}
