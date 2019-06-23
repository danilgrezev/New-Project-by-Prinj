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
    public class CarPartsController : Controller
    {
        private static readonly HashSet<String> AllowedExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png", ".gif" };

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        //private readonly IUserPermissionsService userPermissions;
        private readonly IHostingEnvironment hostingEnvironment;

        public CarPartsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager/*, IUserPermissionsService userPermissions*/, IHostingEnvironment hostingEnvironment)
        {
            this._context = context;
            this.userManager = userManager;
            //this.userPermissions = userPermissions;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: CarParts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = this._context.CarParts
                .Include(c => c.CarGrade);
            return this.View(await applicationDbContext.ToListAsync());
        }

        // GET: CarParts/Details/5
        public async Task<IActionResult> Details(Guid? carPartId)
        {
            if (carPartId == null)
            {
                return this.NotFound();
            }

            var carPart = await this._context.CarParts
                .Include(c => c.CarGrade)
                .Include(b => b.CarDetails)
                .SingleOrDefaultAsync(m => m.Id == carPartId);
            if (carPart == null)
            {
                return this.NotFound();
            }
            this.ViewBag.CarPartId = carPartId;
            return this.View(carPart);
        }
        [Authorize]
        // GET: CarParts/Create
        public async Task<IActionResult> Create(Guid? carGradeId)
        {
            if (carGradeId == null)
            {
                return this.NotFound();
            }
            var carGrade = await this._context.CarGrades
                .SingleOrDefaultAsync(m => m.Id == carGradeId);
            if (carGrade == null)
            {
                return this.NotFound();
            }

            var carGrades = await this._context.CarGrades
                .OrderBy(x => x.Name).ToListAsync();
            this.ViewData["CarGradeId"] = new SelectList(carGrades, "Id", "Name");
            this.ViewBag.CarGrades = carGrade;
            return this.View(new CarPartEditModel());
        }

        // POST: CarParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? carGradeId, CarPartEditModel model)
        {
            if (carGradeId == null)
            {
                this.NotFound();
            }

            var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.PartPath.ContentDisposition).FileName.Value.Trim('"'));
            var fileExt = Path.GetExtension(fileName);

            if (!CarPartsController.AllowedExtensions.Contains(fileExt))
            {
                this.ModelState.AddModelError(nameof(model.PartPath), "This file type is prohibited");
            }

            var carGrade = await this._context.CarGrades
                .SingleOrDefaultAsync(m => m.Id == carGradeId);
            if (carGrade == null)
            {
                this.NotFound();
            }


            if (this.ModelState.IsValid)
            {
                var carPart = new CarPart
                {
                    CarGradeId = carGrade.Id,
                    Name = model.Name,
                    Description = model.Description

                };

                var attachmentPath = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments/parts", carPart.Id.ToString("N") + fileExt);
                carPart.PartPath = $"/attachments/parts/{carPart.Id:N}{fileExt}";
                using (var fileStream = new FileStream(attachmentPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                {
                    await model.PartPath.CopyToAsync(fileStream);
                }

                carPart.Id = Guid.NewGuid();
                this._context.Add(carPart);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Details", "CarGrades", new { carGradeId = carGrade.Id });
            }
            this.ViewData["CarGradeId"] = new SelectList(this._context.CarGrades, "Id", "GradePath");
            this.ViewBag.CarGrades = carGrade;
            return this.View(model);
        }

        // GET: CarParts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var carPart = await this._context.CarParts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carPart == null)
            {
                return this.NotFound();
            }
            this.ViewData["CarGradeId"] = new SelectList(this._context.CarGrades, "Id", "GradePath", carPart.CarGradeId);
            return this.View(carPart);
        }

        // POST: CarParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CarPartEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var carPart = await this._context.CarParts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carPart == null)
            {
                return this.NotFound();
            }
            if (this.ModelState.IsValid)
            {
                carPart.Name = model.Name;
                carPart.Description = model.Description;

                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Details", "CarGrades", new { carGradeId = carPart.CarGradeId });
            }
            this.ViewData["CarGradeId"] = new SelectList(this._context.CarGrades, "Id", "GradePath", carPart.CarGradeId);
            return this.View(model);
        }

        // GET: CarParts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var carPart = await this._context.CarParts
                .Include(c => c.CarGrade)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carPart == null)
            {
                return this.NotFound();
            }

            this.ViewBag.CarGrades = carPart; 
            return this.View(carPart);
        }

        // POST: CarParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid carGradeId)
        {
            if (carGradeId == null)
            {
                return this.NotFound();
            }
            var carPart = await this._context.CarParts
                .Include(c=> c.CarGrade)
                .SingleOrDefaultAsync(m => m.Id == carGradeId);
            if (carPart == null)
            {
                return this.NotFound();
            }
            this._context.CarParts.Remove(carPart);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction("Details", "CarGrades", new { carGradeId = carPart.CarGrade.Id });
        }

        private bool CarPartExists(Guid id)
        {
            return this._context.CarParts.Any(e => e.Id == id);
        }
    }
}
