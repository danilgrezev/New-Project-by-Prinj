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
    public class CarDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CarDetails.Include(c => c.Basket).Include(c => c.CarPart);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CarDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carDetail = await _context.CarDetails
                .Include(c => c.Basket)
                .Include(c => c.CarPart)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carDetail == null)
            {
                return NotFound();
            }

            return View(carDetail);
        }

        // GET: CarDetails/Create
        public IActionResult Create()
        {
            ViewData["BasketId"] = new SelectList(_context.Baskets, "Id", "CreatorId");
            ViewData["CarPartId"] = new SelectList(_context.CarParts, "Id", "Name");
            return View();
        }

        // POST: CarDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarPartId,BasketId,DetailPath,Name,Description,Cost")] CarDetail carDetail)
        {
            if (ModelState.IsValid)
            {
                carDetail.Id = Guid.NewGuid();
                _context.Add(carDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BasketId"] = new SelectList(_context.Baskets, "Id", "CreatorId", carDetail.BasketId);
            ViewData["CarPartId"] = new SelectList(_context.CarParts, "Id", "Name", carDetail.CarPartId);
            return View(carDetail);
        }

        // GET: CarDetails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carDetail = await _context.CarDetails.SingleOrDefaultAsync(m => m.Id == id);
            if (carDetail == null)
            {
                return NotFound();
            }
            ViewData["BasketId"] = new SelectList(_context.Baskets, "Id", "CreatorId", carDetail.BasketId);
            ViewData["CarPartId"] = new SelectList(_context.CarParts, "Id", "Name", carDetail.CarPartId);
            return View(carDetail);
        }

        // POST: CarDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CarPartId,BasketId,DetailPath,Name,Description,Cost")] CarDetail carDetail)
        {
            if (id != carDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarDetailExists(carDetail.Id))
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
            ViewData["BasketId"] = new SelectList(_context.Baskets, "Id", "CreatorId", carDetail.BasketId);
            ViewData["CarPartId"] = new SelectList(_context.CarParts, "Id", "Name", carDetail.CarPartId);
            return View(carDetail);
        }

        // GET: CarDetails/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carDetail = await _context.CarDetails
                .Include(c => c.Basket)
                .Include(c => c.CarPart)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carDetail == null)
            {
                return NotFound();
            }

            return View(carDetail);
        }

        // POST: CarDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var carDetail = await _context.CarDetails.SingleOrDefaultAsync(m => m.Id == id);
            _context.CarDetails.Remove(carDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarDetailExists(Guid id)
        {
            return _context.CarDetails.Any(e => e.Id == id);
        }
    }
}
