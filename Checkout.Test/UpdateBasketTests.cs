using Checkout.Test.Util;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.Test
{
    public class UpdateBasketTests : IClassFixture<ApiFixture>
    {
        private readonly ApiFixture _fixture;

        public UpdateBasketTests(ApiFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, TestPriority(0)]
        public async Task Test_Add_Item_To_Basket()
        {
            var product = await _fixture.ApiClient.GetProduct(6);
            await _fixture.ApiClient.UpdateBasket(product, 3);
            var basket = await _fixture.ApiClient.GetBasket();

            Assert.Single(basket.Items);
            Assert.Equal(3, basket.Items.ToArray()[0].Quantity);
            Assert.Equal(6, basket.Items.ToArray()[0].Product.Id);
        }

        [Fact, TestPriority(1)]
        public async Task Test_Update_Existing_Basket_Item()
        {
            var product = await _fixture.ApiClient.GetProduct(6);
            await _fixture.ApiClient.UpdateBasket(product, 2);
            var basket = await _fixture.ApiClient.GetBasket();

            Assert.Single(basket.Items);
            Assert.Equal(2, basket.Items.ToArray()[0].Quantity);
            Assert.Equal(6, basket.Items.ToArray()[0].Product.Id);
        }


        [Fact, TestPriority(2)]
        public async Task Test_Remove_Basket_Item()
        {
            var product = await _fixture.ApiClient.GetProduct(6);
            await _fixture.ApiClient.UpdateBasket(product, 0);
            var basket = await _fixture.ApiClient.GetBasket();

            Assert.Empty(basket.Items);
        }
    }
}
