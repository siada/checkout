using CheckoutApi;
using CheckoutLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.Test.Util
{
    public class ApiFixture : IDisposable
    {
        private HttpClient _client;

        public CheckoutApiClient ApiClient
        {
            get;
            private set;
        }

        public ApiFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Staging");
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var client = server.CreateClient();
            _client = client;
            var t = Task.Run(InitClient);
            t.Wait();

            ApiClient = new CheckoutApiClient(_client);
        }

        private async Task InitClient()
        {
            var response = await _client.GetAsync("/api/products");
            if(!response.Headers.Contains("X-Basket-Key"))
            {
                throw new InvalidOperationException("API did not return a basket key as expected...");
            }

            var basketKey = response.Headers.GetValues("X-Basket-Key").ToArray()[0];
            _client.DefaultRequestHeaders.Add("BasketKey", basketKey);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
