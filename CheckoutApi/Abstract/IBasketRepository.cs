using Checkout.Shared.Models;
using System.Threading.Tasks;

namespace CheckoutApi.Abstract
{
    public interface IBasketRepository
    {
        Task<string> CreateBasket();

        Task<Basket> GetBasket(string basketKey);

        Task UpdateBasket(string basketKey, Product product, int quantity);

        Task EmptyBasket(string basketKey);
    }
}
