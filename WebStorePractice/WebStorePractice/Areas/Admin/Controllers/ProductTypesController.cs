using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStorePractice.Data;
using WebStorePractice.Models;

namespace WebStorePractice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProductTypesController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View(db.ProductTypes.ToList());
        }

        //GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST CREAT Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                db.Add(productType);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var  productType = await db.ProductTypes.FindAsync(id);
            if(productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        //POST Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductType productType)
        {
            if(id != productType.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var product = await db.ProductTypes.FindAsync(id);
                
                product.Name = productType.Name;
                await db.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = await db.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = await db.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        //POST Delete Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await db.ProductTypes.FindAsync(id);

            db.ProductTypes.Remove(product);

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            
        }
    }
}