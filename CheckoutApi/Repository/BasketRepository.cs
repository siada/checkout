using Checkout.Shared.Models;
using CheckoutApi.Abstract;
using CheckoutApi.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApi.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<BasketRepository> _logger;
        private readonly IProductRepository _productRepository;

        public BasketRepository(
            IDatabaseService databaseService, 
            ILogger<BasketRepository> logger,
            IProductRepository productRepository)
        {
            _databaseService = databaseService;
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<string> CreateBasket()
        {
            string basketKey = null;
            using (var connection = _databaseService.GetDbConnection())
            {
                try
                {
                    await Policy
                        // TODO: Find a more generic way to catch UNIQUE constraint errors across more DBMS types
                        .Handle<SqliteException>(ex => ex.SqliteErrorCode == 19)
                        .RetryAsync(3)
                        .ExecuteAsync(async () =>
                        {
                            basketKey = Guid.NewGuid().ToString();
                            await ExecuteTokenInsert(connection, basketKey);

                        });
                }
                catch (SqliteException ex)
                {
                    _logger.LogWarning(ex, "Generated duplicate basket key, retrying");
                }
            }
            return basketKey;
        }

        public async Task<Basket> GetBasket(string basketKey)
        {
            using (var connection = _databaseService.GetDbConnection())
            {
                var basketId = await GetBasketId(basketKey);
                var basketItems = await connection.QueryAsync<QueryBasketItem>("SELECT * FROM BasketItems WHERE BasketId = @basketId", new
                {
                    basketId
                });
                var basket = new Basket
                {
                    Items = Enumerable.Empty<BasketItem>()
                };
                if (basketItems.Count() == 0) return basket;

                var products = await _productRepository.GetProducts(basketItems.Select(item => item.ProductId).ToArray());
                basket.Items = basketItems.Select(item => new BasketItem
                {
                    Product = products.Single(product => product.Id == item.ProductId),
                    Quantity = item.Quantity
                });
                return basket;
            }
        }

        /// <summary>
        /// Simultaneously updates, creates and removes items from a basket
        ///     - Attempts to update the quantity of a product if it exists in the basket
        ///     - Otherwise inserts into the basket the new product
        ///     - Deletes the product from the basket if the quantity is 0
        /// </summary>
        public async Task UpdateBasket(string basket, Product product, int quantity)
        {
            using (var connection = _databaseService.GetDbConnection())
            {
                var basketId = await GetBasketId(basket);
                await connection.ExecuteAsync(@"
                    UPDATE BasketItems SET Quantity = @quantity WHERE ProductId = @productId AND BasketId = @basketId;
                    INSERT INTO BasketItems(BasketId,ProductId,Quantity) SELECT @basketId, @productId, @quantity WHERE (SELECT changes() = 0);
                    DELETE FROM BasketItems WHERE BasketId = @basketId AND ProductId = @productId AND Quantity = 0", new
                {
                    quantity,
                    productId = product.Id,
                    basketId
                });
            }
        }

        public async Task EmptyBasket(string basketKey)
        {
            using (var connection = _databaseService.GetDbConnection())
            {
                var basketId = await GetBasketId(basketKey);
                await connection.ExecuteAsync("DELETE FROM BasketItems WHERE BasketId = @basketId", new
                {
                    basketId
                });
            }
        }

        private async Task<int> GetBasketId(string basketKey)
        {
            using (var connection = _databaseService.GetDbConnection())
            {
                return await connection.QuerySingleAsync<int>("SELECT Id FROM Baskets WHERE BasketKey = @basketKey", new
                {
                    basketKey
                });
            }
        }

        private async Task ExecuteTokenInsert(DbConnection connection, string basketKey)
        {
            await connection.ExecuteAsync("INSERT INTO Baskets(BasketKey) VALUES (@basketKey)", new
            {
                basketKey
            });
        }
    }
}
