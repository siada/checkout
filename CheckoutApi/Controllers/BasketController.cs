using Checkout.Shared.ViewModels;
using CheckoutApi.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace CheckoutApi.Controllers
{
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var basketKeyHeader = Request.Headers["BasketKey"];
            if (basketKeyHeader == StringValues.Empty)
            {
                throw new InvalidOperationException("BasketKey was missing");
            }

            var basketKey = basketKeyHeader[0];
            return Json(await _basketRepository.GetBasket(basketKey));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]BasketUpdateRequest request)
        {
            var basketKeyHeader = Request.Headers["BasketKey"];
            if (basketKeyHeader == StringValues.Empty)
            {
                throw new InvalidOperationException("BasketKey was missing");
            }

            var basketKey = basketKeyHeader[0];
            await _basketRepository.UpdateBasket(basketKey, request.Product, request.Quantity);

            return Json(new SuccessResponse
            {
                Success = true
            });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var basketKeyHeader = Request.Headers["BasketKey"];
            if (basketKeyHeader == StringValues.Empty)
            {
                throw new InvalidOperationException("BasketKey was missing");
            }

            var basketKey = basketKeyHeader[0];
            await _basketRepository.EmptyBasket(basketKey);

            return Json(new SuccessResponse
            {
                Success = true
            });
        }
    }
}
