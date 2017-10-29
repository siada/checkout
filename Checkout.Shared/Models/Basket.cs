using System.Collections.Generic;

namespace Checkout.Shared.Models
{
    public class Basket
    {
        public IEnumerable<BasketItem> Items { get; set; }
    }
}
