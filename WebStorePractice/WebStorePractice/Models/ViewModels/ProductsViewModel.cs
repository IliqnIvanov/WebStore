using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStorePractice.Models.ViewModels
{
    public class ProductsViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<ProductType> ProductTypes { get; set; }
    }
}
