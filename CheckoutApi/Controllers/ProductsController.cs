using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheckoutApi.Abstract;
using Checkout.Shared.ViewModels;

namespace CheckoutApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IBasketRepository _basketRepository;

        public ProductsController(IProductRepository productRepository, IBasketRepository basketRepository)
        {
            _productRepository = productRepository;
            _basketRepository = basketRepository;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? id = null)
        {
            if (!id.HasValue)
            {

                return Json(new ProductsListResponse
                {
                    Products = await _productRepository.GetProducts()
                });
            } else
            {
                return Json(await _productRepository.GetProduct(id.Value));
            }
        }
    }
}
