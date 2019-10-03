using WebStorePractice.Models.ViewModels;

namespace WebStorePractice.Services
{
    public interface IProductService
    {
        ProductsViewModel GetProductsVM();
    }
}