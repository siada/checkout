using Checkout.Test.Util;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.Test
{
    public class EmptyBasketTests : IClassFixture<ApiFixture>
    {
        private readonly ApiFixture _fixture;

        public EmptyBasketTests(ApiFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Test_Empty_Basket()
        {
            await Test_Basket_Is_Empty();

            var product = await _fixture.ApiClient.GetProduct(1);
            var updateBasketResponse = await _fixture.ApiClient.UpdateBasket(product, 4);
            Assert.True(updateBasketResponse.Success);

            var basket = await _fixture.ApiClient.GetBasket();
            Assert.Single(basket.Items);
            Assert.Equal(4, basket.Items.ToArray()[0].Quantity);

            await _fixture.ApiClient.EmptyBasket();

            await Test_Basket_Is_Empty();
        }

        [Fact]
        public async Task Test_Basket_Is_Empty()
        {
            var basket = await _fixture.ApiClient.GetBasket();
            Assert.Empty(basket.Items);
        }
    }
}
