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
    public class CarGradesController : Controller
    {
        private static readonly HashSet<String> AllowedExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png", ".gif" };

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        //private readonly IUserPermissionsService userPermissions;
        private readonly IHostingEnvironment hostingEnvironment;

        public CarGradesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager/*, IUserPermissionsService userPermissions*/, IHostingEnvironment hostingEnvironment)
        {
            this._context = context;
            this.userManager = userManager;
            //this.userPermissions = userPermissions;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: CarGrades
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CarGrades
                .Include(c => c.CarModel);
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

        [Authorize]
        // GET: CarGrades/Create
        public async Task<IActionResult> Create(Guid? carModelId)
        {
            if (carModelId == null)
            {
                return this.NotFound();
            }

            var carModel = await this._context.CarModels
                .SingleOrDefaultAsync(m => m.Id == carModelId);

            if (carModel == null)
            {
                return this.NotFound();
            }

            var carModels = await this._context.CarModels
                .OrderBy(x => x.Name).ToListAsync();
            ViewData["CarModelId"] = new SelectList(carModels, "Id", "Name");
            this.ViewBag.CarModels = carModel;
            return View(new CarGradeEditModel());
        }

        // POST: CarGrades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? carModelId, CarGradeEditModel model)
        {
            if (carModelId == null)
            {
                this.NotFound();
            }

            var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.GradePath.ContentDisposition).FileName.Value.Trim('"'));
            var fileExt = Path.GetExtension(fileName);

            if (!CarGradesController.AllowedExtensions.Contains(fileExt))
            {
                this.ModelState.AddModelError(nameof(model.GradePath), "This file type is prohibited");
            }

            var carModel = await this._context.CarModels
                .SingleOrDefaultAsync(m => m.Id == carModelId);

            if (carModel == null)
            {
                this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var carGrade = new CarGrade
                {
                    CarModelId = carModel.Id,
                    Name = model.Name,
                    Description = model.Description
                };

                var attachmentPath = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments/grades", carGrade.Id.ToString("N") + fileExt);
                carGrade.GradePath = $"/attachments/grades/{carGrade.Id:N}{fileExt}";
                using (var fileStream = new FileStream(attachmentPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                {
                    await model.GradePath.CopyToAsync(fileStream);
                }


                carGrade.Id = Guid.NewGuid();
                this._context.Add(carGrade);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Details", "CarModels", new { carModelId = carModel.Id });
            }
            ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "ModelPath");
            this.ViewBag.CarModels = carModel;
            return View(model);
        }

        // GET: CarGrades/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carGrade = await _context.CarGrades
                .SingleOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Edit(Guid id, CarGradeEditModel model)
        {
            if (id == null)
            {
                return NotFound();
            }
            var carGrade = await this._context.CarGrades
                .SingleOrDefaultAsync(m => m.Id == id);

            if (carGrade == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                carGrade.Name = model.Name;
                carGrade.Description = model.Description;

                await this._context.SaveChangesAsync();
                return RedirectToAction("Details", "CarModels", new { carModelId = carGrade.CarModelId });
            }
            ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "ModelPath", carGrade.CarModelId);
            return View(model);
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

            this.ViewBag.CarModels = carGrade;
            return View(carGrade);
        }

        // POST: CarGrades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid carModelId)
        {
            if (carModelId == null)
            {
                return this.NotFound();
            }

            var carGrade = await _context.CarGrades
                .Include(c=> c.CarModel)
                .SingleOrDefaultAsync(m => m.Id == carModelId);
            if (carGrade == null)
            {
                return this.NotFound();
            }
            _context.CarGrades.Remove(carGrade);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "CarModels", new { carModelId = carGrade.CarModel.Id });
        }

        private bool CarGradeExists(Guid id)
        {
            return _context.CarGrades.Any(e => e.Id == id);
        }
    }
}
