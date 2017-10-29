using Checkout.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckoutApi.Abstract
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts(params int[] ids);

        Task<Product> GetProduct(int id);
    }
}
