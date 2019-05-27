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
    public class CarPartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarPartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarParts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CarParts.Include(c => c.CarGrade);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CarParts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPart = await _context.CarParts
                .Include(c => c.CarGrade)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carPart == null)
            {
                return NotFound();
            }

            return View(carPart);
        }

        // GET: CarParts/Create
        public IActionResult Create()
        {
            ViewData["CarGradeId"] = new SelectList(_context.CarGrades, "Id", "GradePath");
            return View();
        }

        // POST: CarParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarGradeId,PartPath,Name,Description")] CarPart carPart)
        {
            if (ModelState.IsValid)
            {
                carPart.Id = Guid.NewGuid();
                _context.Add(carPart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarGradeId"] = new SelectList(_context.CarGrades, "Id", "GradePath", carPart.CarGradeId);
            return View(carPart);
        }

        // GET: CarParts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPart = await _context.CarParts.SingleOrDefaultAsync(m => m.Id == id);
            if (carPart == null)
            {
                return NotFound();
            }
            ViewData["CarGradeId"] = new SelectList(_context.CarGrades, "Id", "GradePath", carPart.CarGradeId);
            return View(carPart);
        }

        // POST: CarParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CarGradeId,PartPath,Name,Description")] CarPart carPart)
        {
            if (id != carPart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carPart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarPartExists(carPart.Id))
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
            ViewData["CarGradeId"] = new SelectList(_context.CarGrades, "Id", "GradePath", carPart.CarGradeId);
            return View(carPart);
        }

        // GET: CarParts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carPart = await _context.CarParts
                .Include(c => c.CarGrade)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carPart == null)
            {
                return NotFound();
            }

            return View(carPart);
        }

        // POST: CarParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var carPart = await _context.CarParts.SingleOrDefaultAsync(m => m.Id == id);
            _context.CarParts.Remove(carPart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarPartExists(Guid id)
        {
            return _context.CarParts.Any(e => e.Id == id);
        }
    }
}
