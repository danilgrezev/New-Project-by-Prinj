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
    public class CarModelsController : Controller
    {
        private static readonly HashSet<String> AllowedExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png", ".gif" };

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        //private readonly IUserPermissionsService userPermissions;
        private readonly IHostingEnvironment hostingEnvironment;

        public CarModelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager/*, IUserPermissionsService userPermissions*/, IHostingEnvironment hostingEnvironment)
        {
            this._context = context;
            this.userManager = userManager;
            //this.userPermissions = userPermissions;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: CarModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CarModels
                .Include(c => c.CarBrand);
            return this.View(await applicationDbContext.ToListAsync());
        }

        // GET: CarModels/Details/5
        public async Task<IActionResult> Details(Guid? carModelId)
        {
            if (carModelId == null)
            {
                return this.NotFound();
            }

            var carModel = await this._context.CarModels
                .Include(g => g.CarGrades)
                .SingleOrDefaultAsync(m => m.Id == carModelId);
            if (carModel == null)
            {
                return this.NotFound();
            }

            this.ViewBag.CarModelId = carModelId;
            return this.View(carModel);
        }
        [Authorize]
        // GET: CarModels/Create
        public async Task<IActionResult> Create(Guid? carBrandId)
        {
            if (carBrandId == null)
            {
                return this.NotFound();
            }

            var carBrand = await this._context.CarBrands
                .SingleOrDefaultAsync(m => m.Id == carBrandId);

            if (carBrand == null)
            {
                return this.NotFound();
            }

            var carBrands = await this._context.CarBrands
                .OrderBy(x => x.Name).ToListAsync();

            this.ViewData["CarBrandId"] = new SelectList(carBrands, "Id", "Name");
            this.ViewBag.CarBrands = carBrand;
            return this.View(new CarModelEditModel());
        }        
        

        // POST: CarModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? carBrandId, CarModelEditModel model)
        {
            if (carBrandId == null)
            {                
                this.NotFound();
            }

            var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.ModelPath.ContentDisposition).FileName.Value.Trim('"'));
            var fileExt = Path.GetExtension(fileName);

            if (!CarModelsController.AllowedExtensions.Contains(fileExt))
            {
                this.ModelState.AddModelError(nameof(model.ModelPath), "This file type is prohibited");
            }

            var carBrand = await this._context.CarBrands
                .SingleOrDefaultAsync(m => m.Id == carBrandId);

            if (carBrand == null)
            {
                this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var carModel = new CarModel
                {
                    CarBrandId = carBrand.Id,
                    Name = model.Name,
                    Description = model.Description
                };

                var attachmentPath = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments/models", carModel.Id.ToString("N") + fileExt);
                carModel.ModelPath = $"/attachments/models/{carModel.Id:N}{fileExt}";
                using (var fileStream = new FileStream(attachmentPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                {
                    await model.ModelPath.CopyToAsync(fileStream);
                }


                carModel.Id = Guid.NewGuid();
                this._context.Add(carModel);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Details", "CarBrands", new { carId = carBrand.Id });
            }
            this.ViewData["CarBrandId"] = new SelectList(this._context.CarBrands, "Id", "Name");
            this.ViewBag.CarBrands = carBrand;
            return this.View(model);
        }

        // GET: CarModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var carModel = await this._context.CarModels
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carModel == null)
            {
                return this.NotFound();
            }
            this.ViewData["CarBrandId"] = new SelectList(this._context.CarBrands, "Id", "Name", carModel.CarBrandId);
            return this.View(carModel);
        }

        // POST: CarModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CarModelEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var carModel = await this._context.CarModels
                .SingleOrDefaultAsync(m => m.Id == id);

            if (carModel == null)
            {
                return this.NotFound();
            }
            if (this.ModelState.IsValid)
            {
                carModel.Name = model.Name;
                carModel.Description = model.Description;

                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Details", "CarBrands", new { carId = carModel.CarBrandId });
            }
            this.ViewData["CarBrandId"] = new SelectList(this._context.CarBrands, "Id", "Name", carModel.CarBrandId);
            return this.View(model);
        }

        // GET: CarModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var carModel = await this._context.CarModels                
                .Include(c => c.CarBrand)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carModel == null)
            {
                return this.NotFound();
            }
            this.ViewBag.CarBrands = carModel;
            return this.View(carModel);
        }

        // POST: CarModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid carBrandId)
        {
            if (carBrandId == null)
            {
                return this.NotFound();
            }

            var carModel = await this._context.CarModels
                .Include(c => c.CarBrand)
                .SingleOrDefaultAsync(m => m.Id == carBrandId);
            if (carModel == null)
            {
                return this.NotFound();
            }
            this._context.CarModels.Remove(carModel);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction("Details", "CarBrands", new { carId = carModel.CarBrand.Id });
        }

        private bool CarModelExists(Guid id)
        {
            return this._context.CarModels.Any(e => e.Id == id);
        }
    }
}
