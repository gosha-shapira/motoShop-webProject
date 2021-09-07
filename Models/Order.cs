using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

    public class Order
    {
        public int OrderId { get; set; }

        public int BuyerId { get; set; }

        public int ProductId { get; set; }

        public List<Products> Products { get; set; }

        public List<int> Quantity { get; set; }

        public int TotalPrice { get; set; }

        public String ShippingAdress { get; set; }
    }
