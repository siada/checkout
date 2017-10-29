using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApi.Models
{
    public class QueryBasketItem
    {
        public int Id { get; set; }

        public int BasketId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
