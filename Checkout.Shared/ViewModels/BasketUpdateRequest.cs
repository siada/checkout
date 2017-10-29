using Checkout.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Shared.ViewModels
{
    public class BasketUpdateRequest
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
