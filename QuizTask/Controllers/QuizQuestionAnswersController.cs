using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using QuizTask.Data;
using QuizTask.Models;

namespace QuizTask.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuizQuestionAnswersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizQuestionAnswersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QuizQuestionAnswers
        public async Task<IActionResult> Index()
        {
           
            var applicationDbContext = _context.QuizQuestionAnswers.Include(q => q.CareerLevel).Include(o=>o.QOptions);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QuizQuestionAnswers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizQuestionAnswer = await _context.QuizQuestionAnswers
                .Include(q => q.CareerLevel)
                .FirstOrDefaultAsync(m => m.QuizID == id);
            if (quizQuestionAnswer == null)
            {
                return NotFound();
            }

            return View(quizQuestionAnswer);
        }

        // GET: QuizQuestionAnswers/Create
        public IActionResult Create()
        {
            ViewData["LevelID"] = new SelectList(_context.CareerLevels, "LevelID", "CareerName");
            return View();
        }

        // POST: QuizQuestionAnswers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizID,QuizQno,Question,Answer,LevelID,QuizStatus,QuizDate")] QuizQuestionAnswer quizQuestionAnswer, string quizOptions)
        {

            var options = JsonConvert.DeserializeObject<List<QuizOptions>>(quizOptions);
            if (ModelState.IsValid)
            {
                _context.Add(quizQuestionAnswer);
                await _context.SaveChangesAsync();
                foreach (var option in options)
                {
                    option.QuizID = quizQuestionAnswer.QuizID;
                    _context.Add(option);
                    await _context.SaveChangesAsync();
                }

               
                return RedirectToAction(nameof(Index));
            }
            ViewData["LevelID"] = new SelectList(_context.CareerLevels, "LevelID", "CareerName", quizQuestionAnswer.LevelID);
            return View(quizQuestionAnswer);
        }

        // GET: QuizQuestionAnswers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var quizQuestionAnswer = _context.QuizQuestionAnswers
                  .Include(q => q.CareerLevel) // Include Career Level
                  .FirstOrDefault(q => q.QuizID == id);



            var option = _context.QuizOptions.Where(a => a.QuizID == id).ToList();
            quizQuestionAnswer.QOptions = option;

            if (quizQuestionAnswer == null)
            {
                return NotFound();
            }
            ViewData["LevelID"] = new SelectList(_context.CareerLevels, "LevelID", "CareerName", quizQuestionAnswer.LevelID);
            return View(quizQuestionAnswer);
        }

        // POST: QuizQuestionAnswers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizID,QuizQno,Question,Answer,LevelID,QuizStatus,QuizDate")] QuizQuestionAnswer quizQuestionAnswer,string quizOptions)
        {
            if (id != quizQuestionAnswer.QuizID)
            {
                return NotFound();
            }
            var options = JsonConvert.DeserializeObject<List<QuizOptions>>(quizOptions);

            if (ModelState.IsValid)
            {
              
                try
                {
                    _context.Update(quizQuestionAnswer);
                    await _context.SaveChangesAsync();


                    foreach (var option in options)
                    {
                        if(option.OptID == 0)
                        {
                            option.QuizID = quizQuestionAnswer.QuizID;
                            _context.Add(option);
                            await _context.SaveChangesAsync();

                        }
                        var existingOption = _context.QuizOptions.FirstOrDefault(o => o.OptID == option.OptID);
                        if (existingOption != null)
                        {
                            existingOption.Options = option.Options;
                            _context.QuizOptions.Update(existingOption);
                        }
                    }
                    await _context.SaveChangesAsync();



                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizQuestionAnswerExists(quizQuestionAnswer.QuizID))
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
            ViewData["LevelID"] = new SelectList(_context.CareerLevels, "LevelID", "CareerName", quizQuestionAnswer.LevelID);
            return View(quizQuestionAnswer);
        }

        // GET: QuizQuestionAnswers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizQuestionAnswer = await _context.QuizQuestionAnswers
                .Include(q => q.CareerLevel)
                .FirstOrDefaultAsync(m => m.QuizID == id);
            if (quizQuestionAnswer == null)
            {
                return NotFound();
            }

            return View(quizQuestionAnswer);
        }

        // POST: QuizQuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quizQuestionAnswer = await _context.QuizQuestionAnswers.FindAsync(id);
            if (quizQuestionAnswer != null)
            {
                _context.QuizQuestionAnswers.Remove(quizQuestionAnswer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizQuestionAnswerExists(int id)
        {
            return _context.QuizQuestionAnswers.Any(e => e.QuizID == id);
        }

        [HttpGet]
        public IActionResult QuizData(int id)
        {
            var lMSContext = _context.QuizQuestionAnswers.Include(q => q.CareerLevel).ToList();

            var data = lMSContext.Where(a => a.LevelID == id).ToList();

            return Ok(data);
        }
        [HttpGet]
        public IActionResult QuizDetail()
        {
            var QuizData = _context.QuizQuestionAnswers.Include(q => q.CareerLevel).ToList();

            var Optiondata = _context.QuizOptions.ToList();

            return Ok();
        }


    }
}
