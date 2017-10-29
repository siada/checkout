using Checkout.Shared.Models;
using Checkout.Shared.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutLib
{
    public class CheckoutApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _apiUri;

        public CheckoutApiClient(string apiHost)
        {
            _httpClient = new HttpClient(new HeaderInterceptor());
            _apiUri = new Uri(apiHost);
            _httpClient.BaseAddress = _apiUri;
        }

        public CheckoutApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var productsObj = await GetApi<ProductsListResponse>("api/products");
            return productsObj.Products;
        }

        public async Task<Product> GetProduct(int productId)
        {
            return await GetApi<Product>($"api/products?id={productId}");
        }

        public async Task<SuccessResponse> UpdateBasket(Product product, int quantity)
        {
            var request = new BasketUpdateRequest
            {
                Product = product,
                Quantity = quantity
            };
            return await PutApi<SuccessResponse, BasketUpdateRequest>("api/basket", request);
        }

        public async Task<Basket> GetBasket()
        {
            return await GetApi<Basket>("api/basket");
        }

        public async Task EmptyBasket()
        {
            await DeleteApi("api/basket");
        }

        private async Task<T> GetApi<T>(string path)
        {
            var response = await _httpClient.GetStringAsync(path);
            return JsonConvert.DeserializeObject<T>(response);
        }

        private async Task<TResponse> PutApi<TResponse,TRequest>(string path, TRequest body)
        {
            var content = JsonConvert.SerializeObject(body);
            var message = await _httpClient.PutAsync(path, new StringContent(content, Encoding.UTF8, "application/json"));
            var response = await message.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(response);
        }

        private async Task DeleteApi(string path)
        {
            await _httpClient.DeleteAsync(path);
        }
    }
}
