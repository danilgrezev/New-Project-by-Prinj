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
    public class AttachmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttachmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attachments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Attachments.Include(a => a.CarDetail);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Attachments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .Include(a => a.CarDetail)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        // GET: Attachments/Create
        public IActionResult Create()
        {
            ViewData["CarDetailId"] = new SelectList(_context.CarDetails, "Id", "DetailPath");
            return View();
        }

        // POST: Attachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarDetailId,FilePath")] Attachment attachment)
        {
            if (ModelState.IsValid)
            {
                attachment.Id = Guid.NewGuid();
                _context.Add(attachment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarDetailId"] = new SelectList(_context.CarDetails, "Id", "DetailPath", attachment.CarDetailId);
            return View(attachment);
        }

        // GET: Attachments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments.SingleOrDefaultAsync(m => m.Id == id);
            if (attachment == null)
            {
                return NotFound();
            }
            ViewData["CarDetailId"] = new SelectList(_context.CarDetails, "Id", "DetailPath", attachment.CarDetailId);
            return View(attachment);
        }

        // POST: Attachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CarDetailId,FilePath")] Attachment attachment)
        {
            if (id != attachment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attachment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttachmentExists(attachment.Id))
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
            ViewData["CarDetailId"] = new SelectList(_context.CarDetails, "Id", "DetailPath", attachment.CarDetailId);
            return View(attachment);
        }

        // GET: Attachments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .Include(a => a.CarDetail)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        // POST: Attachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var attachment = await _context.Attachments.SingleOrDefaultAsync(m => m.Id == id);
            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttachmentExists(Guid id)
        {
            return _context.Attachments.Any(e => e.Id == id);
        }
    }
}
