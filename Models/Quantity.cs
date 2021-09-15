using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class Quantity
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Amount { get; set; }
    }
}