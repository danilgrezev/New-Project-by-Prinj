using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend6.Data;
using Backend6.Models;
using Backend6.Models.ViewModels;
//using Backend6.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;

namespace Backend6.Controllers
{
    public class CarDetailsController : Controller
    {
        private static readonly HashSet<String> AllowedExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png", ".gif" };

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        //private readonly IUserPermissionsService userPermissions;
        private readonly IHostingEnvironment hostingEnvironment;

        public CarDetailsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager/*, IUserPermissionsService userPermissions*/, IHostingEnvironment hostingEnvironment)
        {
            this._context = context;
            this.userManager = userManager;
            //this.userPermissions = userPermissions;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: CarDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CarDetails.Include(c => c.Basket).Include(c => c.CarPart);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CarDetails/Details/5
        public async Task<IActionResult> Details(Guid? carDetailId)
        {
            if (carDetailId == null)
            {
                return NotFound();
            }

            var carDetail = await _context.CarDetails
                .Include(c => c.Basket)
                .Include(c => c.CarPart)
                .SingleOrDefaultAsync(m => m.Id == carDetailId);
            if (carDetail == null)
            {
                return NotFound();
            }
            this.ViewBag.CarDetailId = carDetailId;
            return View(carDetail);
        }
        [Authorize]
        // GET: CarDetails/Create
        public async Task<IActionResult> Create(Guid? carPartId)
        {
            if (carPartId == null)
            {
                return this.NotFound();
            }
            var carPart = await this._context.CarParts
                .SingleOrDefaultAsync(m => m.Id == carPartId);
            if (carPart == null)
            {
                return this.NotFound();
            }
            var carParts = await this._context.CarParts
                .OrderBy(x => x.Name).ToListAsync();
            var baskets = await this._context.Baskets
               .OrderBy(x => x.FullPrice).ToListAsync();
            ViewData["BasketId"] = new SelectList(baskets, "Id", "CreatorId");
            ViewData["CarPartId"] = new SelectList(carParts, "Id", "Name");
            this.ViewBag.CarParts = carPart;
            return View(new CarDetailEditModel());
        }

        // POST: CarDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? carPartId, CarDetailEditModel model)
        {
            if (carPartId == null)
            {
                this.NotFound();
            }

            var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.DetailPath.ContentDisposition).FileName.Value.Trim('"'));
            var fileExt = Path.GetExtension(fileName); 
            if (!CarDetailsController.AllowedExtensions.Contains(fileExt))
            {
                this.ModelState.AddModelError(nameof(model.DetailPath), "This file type is prohibited");
            }

            var carPart = await this._context.CarParts                
                .SingleOrDefaultAsync(m => m.Id == carPartId);
            //var basket = await this._context.Baskets
            //    .SingleOrDefaultAsync(m => m.Id == carPartId);
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            if (carPart == null)
            {
                this.NotFound();
            }

            if (ModelState.IsValid)
            {

                var carDetail = new CarDetail
                {
                    CarPartId = carPart.Id,
                    //BasketId = ,
                    Name = model.Name,
                    Description = model.Description,
                    Cost = model.Cost
                };

                var attachmentPath = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments/parts", carDetail.Id.ToString("N") + fileExt);
                carDetail.DetailPath = $"/attachments/parts/{carDetail.Id:N}{fileExt}";
                using (var fileStream = new FileStream(attachmentPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                {
                    await model.DetailPath.CopyToAsync(fileStream);
                }

                carDetail.Id = Guid.NewGuid();
                _context.Add(carDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "CarParts", new { carPartId = carPart.Id });
            }
            ViewData["BasketId"] = new SelectList(_context.Baskets, "Id", "CreatorId");
            ViewData["CarPartId"] = new SelectList(_context.CarParts, "Id", "Name");
            this.ViewBag.CarParts = carPart;
            return View(model);
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
