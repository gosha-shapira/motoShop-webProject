using motoShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Views.Orders
{
    public class OrderProducts
    {
        public IEnumerable<Products> Products { get; set; }
    }
}
