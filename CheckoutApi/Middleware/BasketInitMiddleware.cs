using CheckoutApi.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace CheckoutApi.Middleware
{
    public class BasketInitMiddleware
    {
        private const string BasketKeyName = "BasketKey";
        private readonly RequestDelegate _next;
        private readonly IBasketRepository _basketRepository;

        public BasketInitMiddleware(RequestDelegate next, IBasketRepository basketRepository)
        {
            _next = next;
            _basketRepository = basketRepository;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Headers[BasketKeyName] == StringValues.Empty)
            {
                var basketKey = await _basketRepository.CreateBasket();
                context.Request.Headers.Add(BasketKeyName, basketKey);
                context.Response.OnStarting(state =>
                {
                    var ctx = (HttpContext)state;
                    var requestHeader = ctx.Request.Headers[BasketKeyName];
                    ctx.Response.Headers.Add("X-Basket-Key", requestHeader);
                    return Task.FromResult(true);
                }, context);
            }
            await _next.Invoke(context);
        }
    }
}
