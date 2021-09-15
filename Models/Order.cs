using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int BuyerId { get; set; }

        public IEnumerable<Quantity> ProductsList { get; set; }

        public int TotalPrice { get; set; }

        public String ShippingAdress { get; set; }
    }
