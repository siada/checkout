using CheckoutApi.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Checkout.Shared.Models;

namespace CheckoutApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDatabaseService _databaseService;

        public ProductRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }



        public async Task<IEnumerable<Product>> GetProducts(params int[] ids)
        {
            using (var connection = _databaseService.GetDbConnection())
            {
                var queryStr = ids.Length == 0
                    ? "SELECT * FROM Products"
                    : "SELECT* FROM Products WHERE Id IN @ids";

                return await connection.QueryAsync<Product>(queryStr, new
                {
                    ids
                });
            }
        }

        public async Task<Product> GetProduct(int id) => (await GetProducts(id)).ToArray()[0];

    }
}
