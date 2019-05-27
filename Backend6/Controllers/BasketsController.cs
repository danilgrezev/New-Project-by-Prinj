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
    public class BasketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BasketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Baskets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Baskets.Include(b => b.Creator);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Baskets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets
                .Include(b => b.Creator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (basket == null)
            {
                return NotFound();
            }

            return View(basket);
        }

        // GET: Baskets/Create
        public IActionResult Create()
        {
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Baskets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatorId,FullPrice")] Basket basket)
        {
            if (ModelState.IsValid)
            {
                basket.Id = Guid.NewGuid();
                _context.Add(basket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", basket.CreatorId);
            return View(basket);
        }

        // GET: Baskets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets.SingleOrDefaultAsync(m => m.Id == id);
            if (basket == null)
            {
                return NotFound();
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", basket.CreatorId);
            return View(basket);
        }

        // POST: Baskets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CreatorId,FullPrice")] Basket basket)
        {
            if (id != basket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasketExists(basket.Id))
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
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", basket.CreatorId);
            return View(basket);
        }

        // GET: Baskets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets
                .Include(b => b.Creator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (basket == null)
            {
                return NotFound();
            }

            return View(basket);
        }

        // POST: Baskets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var basket = await _context.Baskets.SingleOrDefaultAsync(m => m.Id == id);
            _context.Baskets.Remove(basket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BasketExists(Guid id)
        {
            return _context.Baskets.Any(e => e.Id == id);
        }
    }
}
