using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using QuizTask.Models.ViewModel;

namespace QuizTask.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;

        public AdminUsersController(ApplicationDbContext context,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger )
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        [BindProperty]
        public string ReturnUrl { get; set; }

        private List<AuthenticationScheme> ExternalLogins;

        // GET: AdminUsers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AdminUsers.Include(a => a.IdentityUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AdminUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers
                .Include(a => a.IdentityUser)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (adminUser == null)
            {
                return NotFound();
            }

            return View(adminUser);
        }

        // GET: AdminUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,IdentityUserId,User_Reg_No,User_Name,Role,Mobile_No,User_Email,User_Passward,User_Status,User_Date")] AdminUser adminUser, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var user = new IdentityUser { UserName = adminUser.User_Email, Email = adminUser.User_Email, PhoneNumber = adminUser.Mobile_No, EmailConfirmed = true };
            adminUser.IdentityUser = user;
            adminUser.IdentityUserId = adminUser.IdentityUser.Id;
            var result = await _userManager.CreateAsync(user, adminUser.User_Passward);
            if (result.Succeeded)
            {
                await _roleManager.CreateAsync(new IdentityRole(adminUser.Role));
                await _userManager.AddToRoleAsync(user, adminUser.Role);           
      
                _context.Add(adminUser);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User created a new account with password.");
                return RedirectToAction(nameof(Index));
            
            }
            return View(adminUser);
        }

        // GET: AdminUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers.FindAsync(id);
            if (adminUser == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", adminUser.IdentityUserId);
            return View(adminUser);
        }

        // POST: AdminUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,IdentityUserId,User_Reg_No,User_Name,Role,Mobile_No,User_Email,User_Passward,User_Status,User_Date")] AdminUser adminUser)
        {
            if (id != adminUser.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminUserExists(adminUser.UserId))
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
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", adminUser.IdentityUserId);
            return View(adminUser);
        }

        // GET: AdminUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUsers
                .Include(a => a.IdentityUser)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (adminUser == null)
            {
                return NotFound();
            }

            return View(adminUser);
        }

        // POST: AdminUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminUser = await _context.AdminUsers.FindAsync(id);
            if (adminUser != null)
            {
                _context.AdminUsers.Remove(adminUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminUserExists(int id)
        {
            return _context.AdminUsers.Any(e => e.UserId == id);
        }

        public async Task<IActionResult> AdminIndex()
        {

            AdminIndexVM adminVM = new AdminIndexVM();

            var quizQA = _context.QuizQuestionAnswers.Include(a => a.CareerLevel).ToList();
            var testLink = _context.TestLinks.Include(a=>a.CandidateDetail).ToList();
            var candidate = _context.CandidateDetails.Include(a=>a.CareerLevel).ToList();
            var testEmail = _context.EmailDetails.Include(a=>a.TestLinks).ToList();
            var attemptQuiz = _context.QuizAttempts.Include(a=>a.CandidateDetail).ToList();


            adminVM.quizQuestionAnswers = quizQA.OrderByDescending(a => a.QuizID).Take(5).ToList();
            adminVM.testLinks = testLink.OrderByDescending(a => a.TestLinkId).Take(5).ToList();
            adminVM.candidateDetails = candidate.OrderByDescending(a => a.CandidateID).Take(5).ToList();
            adminVM.emailDetails = testEmail.OrderByDescending(a => a.EmailID).Take(5).ToList();
            adminVM.quizAttempts = attemptQuiz.OrderByDescending(a => a.QuizAtmp_ID).Take(5).ToList();

            adminVM.CandidateCount = candidate.Count();
            adminVM.EmailCount = testEmail.Count();
            adminVM.TestLinkCount = testLink.Count();
            adminVM.AttemptQuizCount = attemptQuiz.Count();




            return View(adminVM);
        }

        public async Task<IActionResult> Canditate([Optional] int id, [Optional] string name, [Optional] string email, [Optional] string mobile, [Optional] string status, [Optional] int levelId)
        {
            var result = await _context.CandidateDetails.Include(a => a.CareerLevel).ToListAsync();
            if (id != null && id > 0)
            {
                result = result.Where(a => a.CandidateID == id).ToList();
                return View(result);
            }
            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(a => a.Name == name).ToList();
                return View(result);
            }
            if (!string.IsNullOrEmpty(email))
            {
                result = result.Where(a => a.Candidate_Email == email).ToList();
                return View(result);
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                result = result.Where(a => a.Mobile_No == mobile).ToList();
                return View(result);
            }
            if (!string.IsNullOrEmpty(status))
            {
                result = result.Where(a => a.Status.Contains(status)).ToList();
                return View(result);
            }
            if (levelId != null && levelId > 0)
            {
                result = result.Where(a => a.LevelID == levelId).ToList();
                return View(result);
            }
            var aquery = from a in result where a.IsActive == true select a;
            ViewData["LevelID"] = new SelectList(_context.CareerLevels, "LevelID", "CareerName");
            return View(aquery);
        }

        public async Task<IActionResult> AllTestLink([Optional] int id, [Optional] string status)
        {
            var result = await _context.TestLinks.Include(a => a.CandidateDetail).ToListAsync();
            if (id != null && id > 0)
            {
                result = result.Where(a => a.CandidateID == id).ToList();
                return View(result);
            }
            if (!string.IsNullOrEmpty(status))
            {
                result = result.Where(a => a.Status.Contains(status)).ToList();
                return View(result);
            }
       
            var aquery = from a in result select a;
            return View(aquery);
        }

        public async Task<IActionResult> AllQuizAns([Optional] int levelid)
        {
            var result = await _context.QuizQuestionAnswers.Include(a => a.CareerLevel).ToListAsync();
            if (levelid != null && levelid > 0)
            {
                result = result.Where(a => a.LevelID == levelid).ToList();
                return View(result);
            }

            var aquery = from a in result where a.QuizStatus == true select a;
            ViewData["LevelID"] = new SelectList(_context.CareerLevels, "LevelID", "CareerName");
            return View(aquery);
        }
    }
}
