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

namespace QuizTask.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CareerLevelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CareerLevelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CareerLevels
        public async Task<IActionResult> Index()
        {
            return View(await _context.CareerLevels.ToListAsync());
        }

        // GET: CareerLevels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var careerLevel = await _context.CareerLevels
                .FirstOrDefaultAsync(m => m.LevelID == id);
            if (careerLevel == null)
            {
                return NotFound();
            }

            return View(careerLevel);
        }

        // GET: CareerLevels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CareerLevels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LevelID,CareerName,isActive")] CareerLevel careerLevel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(careerLevel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(careerLevel);
        }

        // GET: CareerLevels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var careerLevel = await _context.CareerLevels.FindAsync(id);
            if (careerLevel == null)
            {
                return NotFound();
            }
            return View(careerLevel);
        }

        // POST: CareerLevels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LevelID,CareerName,isActive")] CareerLevel careerLevel)
        {
            if (id != careerLevel.LevelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(careerLevel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CareerLevelExists(careerLevel.LevelID))
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
            return View(careerLevel);
        }

        // GET: CareerLevels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var careerLevel = await _context.CareerLevels
                .FirstOrDefaultAsync(m => m.LevelID == id);
            if (careerLevel == null)
            {
                return NotFound();
            }

            return View(careerLevel);
        }

        // POST: CareerLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var careerLevel = await _context.CareerLevels.FindAsync(id);
            if (careerLevel != null)
            {
                _context.CareerLevels.Remove(careerLevel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CareerLevelExists(int id)
        {
            return _context.CareerLevels.Any(e => e.LevelID == id);
        }
    }
}
