    using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
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
    public class EmailDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public EmailDetailsController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: EmailDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EmailDetails.Include(e => e.TestLinks);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EmailDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailDetail = await _context.EmailDetails
                .Include(e => e.TestLinks)
                .FirstOrDefaultAsync(m => m.EmailID == id);
            if (emailDetail == null)
            {
                return NotFound();
            }

            return View(emailDetail);
        }

        // GET: EmailDetails/Create
        public IActionResult Create()
        {
            ViewData["TestLinkId"] = new SelectList(_context.TestLinks.Include(a=>a.CandidateDetail).Where(a=>a.Status.Contains("LinkCreate")), "TestLinkId", "TestLinkId");
            return View();
        }

        // POST: EmailDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmailID,TestLinkId,Confirmed_Email,Subject,body,Created_Date,Created_By")] EmailDetail emailDetail)
        {
            if (string.IsNullOrEmpty(emailDetail.Confirmed_Email))
            {
                return BadRequest("Email and Link are required.");
            }
            string userId = HttpContext.Session.GetString("UserId");
            string username = HttpContext.Session.GetString("Username");

            string subject = emailDetail.Subject;
            string body = emailDetail.body;
            emailDetail.Created_Date = DateTime.Now;
            emailDetail.Created_By = Convert.ToInt32(userId);

            bool response =  await _emailService.SendEmailAsync(emailDetail.Confirmed_Email, subject, body);

            if (response)  // Assuming SendEmailAsync() returns a bool (true = success, false = failure)
            {
                if (ModelState.IsValid)
                {
                    var testlink = _context.TestLinks.Where(a => a.TestLinkId == emailDetail.TestLinkId).FirstOrDefault();
                    testlink.Status = "LinkSend";
                    _context.Update(testlink);

                    _context.Add(emailDetail);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return StatusCode(500, new { success = false, error = "Failed to send email. Please try again later." });
            }




            ViewData["TestLinkId"] = new SelectList(_context.TestLinks, "TestLinkId", "TestLinkId", emailDetail.TestLinkId);
            return View(emailDetail);
        }

        // GET: EmailDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailDetail = await _context.EmailDetails.FindAsync(id);
            if (emailDetail == null)
            {
                return NotFound();
            }
            ViewData["TestLinkId"] = new SelectList(_context.TestLinks, "TestLinkId", "TestLinkId", emailDetail.TestLinkId);
            return View(emailDetail);
        }

        // POST: EmailDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmailID,TestLinkId,Confirmed_Email,Subject,body,Created_Date,Created_By")] EmailDetail emailDetail)
        {
            if (id != emailDetail.EmailID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emailDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailDetailExists(emailDetail.EmailID))
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
            ViewData["TestLinkId"] = new SelectList(_context.TestLinks, "TestLinkId", "TestLinkId", emailDetail.TestLinkId);
            return View(emailDetail);
        }

        // GET: EmailDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailDetail = await _context.EmailDetails
                .Include(e => e.TestLinks)
                .FirstOrDefaultAsync(m => m.EmailID == id);
            if (emailDetail == null)
            {
                return NotFound();
            }

            return View(emailDetail);
        }

        // POST: EmailDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emailDetail = await _context.EmailDetails.FindAsync(id);
            if (emailDetail != null)
            {
                _context.EmailDetails.Remove(emailDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailDetailExists(int id)
        {
            return _context.EmailDetails.Any(e => e.EmailID == id);
        }
        [HttpGet]
        public async Task<IActionResult> getTestLink(int linkId)
        {
            var testlink = await _context.TestLinks.Include(a => a.CandidateDetail).Where(a => a.TestLinkId == linkId).FirstOrDefaultAsync();
            return Ok(testlink);
        }
    }
}
