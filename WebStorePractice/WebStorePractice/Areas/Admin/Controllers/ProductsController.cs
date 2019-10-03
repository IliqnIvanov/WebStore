using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStorePractice.Data;
using WebStorePractice.Models;
using WebStorePractice.Models.ViewModels;
using WebStorePractice.Utilities;

namespace WebStorePractice.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment hostingEnvironment;

        [BindProperty]
        public ProductsViewModel ProductsViewModel { get; set; }

        public ProductsController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            this.db = db;
            this.hostingEnvironment = hostingEnvironment;
            this.ProductsViewModel = new ProductsViewModel()
            {
                Product = new Product(),
                ProductTypes = db.ProductTypes.ToList()
            };
        }

        public async Task<IActionResult> Index()
        {
            var products = db.Products.Include(m => m.ProductType);
            return View(await products.ToListAsync());
        }

        //GET Products Create
        public IActionResult Create()
        {
            return View(ProductsViewModel);
        }

        //Post Products Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
            {
                return View(ProductsViewModel);
            }

            db.Add(ProductsViewModel.Product);
            await db.SaveChangesAsync();

            //Image being saved

            string webRootPath = this.hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var productsFromDb = this.db.Products.Find(ProductsViewModel.Product.Id);

            if (files.Count != 0)
            {
                //Image has been uploaded
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploads,
                    ProductsViewModel.Product.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\"
                    + ProductsViewModel.Product.Id + extension;
            }
            else
            {
                //When used does not upload image
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + 
                    @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPath +
                    @"\" + SD.ImageFolder + @"\" + ProductsViewModel.Product.Id + ".jpg");

                productsFromDb.Image = @"\" + SD.ImageFolder + @"\"
                    + ProductsViewModel.Product.Id + ".jpg";
            }
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ProductsViewModel.Product = await db.Products.Include(m => m.ProductType).SingleOrDefaultAsync(m => m.Id == id);

            if(ProductsViewModel.Product == null)
            {
                return NotFound();
            }

            return View(ProductsViewModel);
        }


        //POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                string webRoothPath = hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var productFromDb = db.Products.Where(m => m.Id == ProductsViewModel.Product.Id).FirstOrDefault();

                if(files.Count > 0 && files[0] != null)
                {
                    var uploads = Path.Combine(webRoothPath, SD.ImageFolder);
                    var extensionNew = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(productFromDb.Image);

                    if (System.IO.File.Exists(Path.Combine(uploads,ProductsViewModel.Product.Id + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, ProductsViewModel.Product.Id + extension_old));
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploads,
                    ProductsViewModel.Product.Id + extensionNew), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    ProductsViewModel.Product.Image = @"\" + SD.ImageFolder + @"\"
                        + ProductsViewModel.Product.Id + extensionNew;
                }

                if (ProductsViewModel.Product.Image != null)
                {
                    productFromDb.Image = ProductsViewModel.Product.Image;
                }

                productFromDb.Name = ProductsViewModel.Product.Name;
                productFromDb.Price = ProductsViewModel.Product.Price;
                productFromDb.Available = ProductsViewModel.Product.Available;
                productFromDb.ProductTypeId = ProductsViewModel.Product.ProductTypeId;
                productFromDb.SpecialTag = ProductsViewModel.Product.SpecialTag;

                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            return View(ProductsViewModel);
        }

        //GET Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductsViewModel.Product = await db.Products.Include(m => m.ProductType).SingleOrDefaultAsync(m => m.Id == id);

            if (ProductsViewModel.Product == null)
            {
                return NotFound();
            }

            return View(ProductsViewModel);
        }

        //GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductsViewModel.Product = await db.Products.Include(m => m.ProductType).SingleOrDefaultAsync(m => m.Id == id);

            if (ProductsViewModel.Product == null)
            {
                return NotFound();
            }

            return View(ProductsViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webPath = hostingEnvironment.WebRootPath;

            var product = await db.Products.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }
            else
            {
                var uploads = Path.Combine(webPath, SD.ImageFolder);
                var extension = Path.GetExtension(product.Image);

                if(System.IO.File.Exists(Path.Combine(uploads, product.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, product.Id + extension));
                }
                db.Products.Remove(product);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }

    }
}