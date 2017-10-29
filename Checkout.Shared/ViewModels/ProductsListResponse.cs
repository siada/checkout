using Checkout.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Shared.ViewModels
{
    public class ProductsListResponse
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
