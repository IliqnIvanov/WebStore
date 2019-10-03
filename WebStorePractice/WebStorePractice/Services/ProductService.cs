using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStorePractice.Data;
using WebStorePractice.Models;
using WebStorePractice.Models.ViewModels;

namespace WebStorePractice.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext db;

        public ProductService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public ProductsViewModel GetProductsVM()
        {
            
            return new ProductsViewModel()
            {
                Product = new Product(),
                ProductTypes = db.ProductTypes.ToList()
            };
        }
    }
}
