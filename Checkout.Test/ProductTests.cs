using Checkout.Test.Util;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.Test
{
    public class ProductTests : IClassFixture<ApiFixture>
    {
        private readonly ApiFixture _fixture;

        public ProductTests(ApiFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Test_GetProducts()
        {
            var products = await _fixture.ApiClient.GetProducts();
            Assert.Equal(10, products.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public async Task Test_GetProduct_By_Id(int productId)
        {
            var product = await _fixture.ApiClient.GetProduct(productId);
            Assert.Equal(productId, product.Id);
        }
    }
}
