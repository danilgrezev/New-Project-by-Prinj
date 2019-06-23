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
    public class CarBrandsController : Controller
    {
        private static readonly HashSet<String> AllowedExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png", ".gif" };

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        //private readonly IUserPermissionsService userPermissions;
        private readonly IHostingEnvironment hostingEnvironment;

        public CarBrandsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager/*, IUserPermissionsService userPermissions*/, IHostingEnvironment hostingEnvironment)
        {
            this._context = context;
            this.userManager = userManager;
            //this.userPermissions = userPermissions;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: CarBrands
        public async Task<IActionResult> Index()
        {
            return this.View(await this._context.CarBrands.ToListAsync());
        }

        // GET: CarBrands/Details/5
        public async Task<IActionResult> Details(Guid? carId)
        {
            if (carId == null)
            {
                return this.NotFound();
            }

            var carBrand = await this._context.CarBrands
                .Include(f => f.CarModels)
                .SingleOrDefaultAsync(m => m.Id == carId);
            if (carBrand == null)
            {
                return this.NotFound();
            }
            this.ViewBag.CarBrandId = carId;
            return this.View(carBrand);
        }
        [Authorize]
        // GET: CarBrands/Create
        public IActionResult Create()
        {
            return this.View(new CarBrandEditModel());
        }

        // POST: CarBrands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CarBrandEditModel model)
        {
            var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.PathBrand.ContentDisposition).FileName.Value.Trim('"'));
            var fileExt = Path.GetExtension(fileName);

            if (!CarBrandsController.AllowedExtensions.Contains(fileExt))
            {
                this.ModelState.AddModelError(nameof(model.PathBrand), "This file type is prohibited");
            }


            if (this.ModelState.IsValid)
            {
                var carBrand = new CarBrand
                {
                    Name = model.Name,
                    Description = model.Description                    

                };

                var attachmentPath = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments/brands", carBrand.Id.ToString("N") + fileExt);
                carBrand.PathBrand = $"/attachments/brands/{carBrand.Id:N}{fileExt}";
                using (var fileStream = new FileStream(attachmentPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                {
                    await model.PathBrand.CopyToAsync(fileStream);
                }


                carBrand.Id = Guid.NewGuid();
                this._context.Add(carBrand);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }
            return this.View(model);
        }

        // GET: CarBrands/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var carBrand = await this._context.CarBrands.SingleOrDefaultAsync(m => m.Id == id);
            if (carBrand == null)
            {
                return this.NotFound();
            }
            var model = new CarBrandEditModel
            {
                Name = carBrand.Name,
                Description = carBrand.Description
            };

            this.ViewBag.CarBrand = carBrand;
            return this.View(model);
        }

        // POST: CarBrands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, CarBrandEditModel model)
        {
            var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.PathBrand.ContentDisposition).FileName.Value.Trim('"'));
            var fileExt = Path.GetExtension(fileName);

            if (!CarBrandsController.AllowedExtensions.Contains(fileExt))
            {
                this.ModelState.AddModelError(nameof(model.PathBrand), "This file type is prohibited");
            }

            if (id == null)
            {
                return this.NotFound();
            }
            var carBrand = await this._context.CarBrands.SingleOrDefaultAsync(m => m.Id == id);
            if (carBrand == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var attachmentPath = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments", carBrand.Id.ToString("N") + fileExt);
                carBrand.PathBrand = $"/attachments/{carBrand.Id:N}{fileExt}";
                using (var fileStream = new FileStream(attachmentPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                {
                    await model.PathBrand.CopyToAsync(fileStream);
                }

                carBrand.Name = model.Name;
                carBrand.Description = model.Description;

                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            this.ViewBag.CarBrand = carBrand;
            return this.View(model);
        }

        // GET: CarBrands/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var carBrand = await this._context.CarBrands
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carBrand == null)
            {
                return this.NotFound();
            }

            return this.View(carBrand);
        }

        // POST: CarBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var carBrand = await this._context.CarBrands.SingleOrDefaultAsync(m => m.Id == id);
            this._context.CarBrands.Remove(carBrand);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }

        private bool CarBrandExists(Guid id)
        {
            return this._context.CarBrands.Any(e => e.Id == id);
        }
    }
}
