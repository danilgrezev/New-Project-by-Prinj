using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend6.Data;
using Backend6.Models;

namespace Backend6.Controllers
{
    public class CarGradesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarGradesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarGrades
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CarGrades.Include(c => c.CarModel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CarGrades/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carGrade = await _context.CarGrades
                .Include(c => c.CarModel)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carGrade == null)
            {
                return NotFound();
            }

            return View(carGrade);
        }

        // GET: CarGrades/Create
        public IActionResult Create()
        {
            ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "ModelPath");
            return View();
        }

        // POST: CarGrades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarModelId,GradePath,Name,Description")] CarGrade carGrade)
        {
            if (ModelState.IsValid)
            {
                carGrade.Id = Guid.NewGuid();
                _context.Add(carGrade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "ModelPath", carGrade.CarModelId);
            return View(carGrade);
        }

        // GET: CarGrades/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carGrade = await _context.CarGrades.SingleOrDefaultAsync(m => m.Id == id);
            if (carGrade == null)
            {
                return NotFound();
            }
            ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "ModelPath", carGrade.CarModelId);
            return View(carGrade);
        }

        // POST: CarGrades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CarModelId,GradePath,Name,Description")] CarGrade carGrade)
        {
            if (id != carGrade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carGrade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarGradeExists(carGrade.Id))
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
            ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "ModelPath", carGrade.CarModelId);
            return View(carGrade);
        }

        // GET: CarGrades/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carGrade = await _context.CarGrades
                .Include(c => c.CarModel)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carGrade == null)
            {
                return NotFound();
            }

            return View(carGrade);
        }

        // POST: CarGrades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var carGrade = await _context.CarGrades.SingleOrDefaultAsync(m => m.Id == id);
            _context.CarGrades.Remove(carGrade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarGradeExists(Guid id)
        {
            return _context.CarGrades.Any(e => e.Id == id);
        }
    }
}
