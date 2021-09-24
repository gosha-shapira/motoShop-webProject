using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int BuyerId { get; set; }

        
        //public Users User{ get; set; } //insted of BuyerId, use User and UserId for an optional join query
        //public int UserId{ get; set; }

        // should be Product
        public IEnumerable<Quantity> ProductsList { get; set; } // consider removing this

        public int TotalPrice { get; set; }

        public String ShippingAdress { get; set; }
    }
}