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
    public class CarModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CarModels.Include(c => c.CarBrand);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CarModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.CarModels
                .Include(c => c.CarBrand)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carModel == null)
            {
                return NotFound();
            }

            return View(carModel);
        }

        // GET: CarModels/Create
        public IActionResult Create()
        {
            ViewData["CarBrandId"] = new SelectList(_context.CarBrands, "Id", "Name");
            return View();
        }

        // POST: CarModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarBrandId,ModelPath,Name,Description")] CarModel carModel)
        {
            if (ModelState.IsValid)
            {
                carModel.Id = Guid.NewGuid();
                _context.Add(carModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarBrandId"] = new SelectList(_context.CarBrands, "Id", "Name", carModel.CarBrandId);
            return View(carModel);
        }

        // GET: CarModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.CarModels.SingleOrDefaultAsync(m => m.Id == id);
            if (carModel == null)
            {
                return NotFound();
            }
            ViewData["CarBrandId"] = new SelectList(_context.CarBrands, "Id", "Name", carModel.CarBrandId);
            return View(carModel);
        }

        // POST: CarModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CarBrandId,ModelPath,Name,Description")] CarModel carModel)
        {
            if (id != carModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarModelExists(carModel.Id))
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
            ViewData["CarBrandId"] = new SelectList(_context.CarBrands, "Id", "Name", carModel.CarBrandId);
            return View(carModel);
        }

        // GET: CarModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carModel = await _context.CarModels
                .Include(c => c.CarBrand)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carModel == null)
            {
                return NotFound();
            }

            return View(carModel);
        }

        // POST: CarModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var carModel = await _context.CarModels.SingleOrDefaultAsync(m => m.Id == id);
            _context.CarModels.Remove(carModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarModelExists(Guid id)
        {
            return _context.CarModels.Any(e => e.Id == id);
        }
    }
}
