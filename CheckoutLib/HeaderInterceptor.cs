using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CheckoutLib
{
    public class HeaderInterceptor : DelegatingHandler
    {
        private string basketKey = null;
        public HeaderInterceptor()
        {
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (basketKey != null) request.Headers.Add("BasketKey", basketKey);

            var response = await base.SendAsync(request, cancellationToken);
            if(response.Headers.Contains("X-Basket-Key"))
                basketKey = response.Headers.GetValues("X-Basket-Key").ToArray()[0];
            
            return response;
        }
    }
}
