using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class Order
    {
        [Key]
        [Display(Name = "Order ID")]
        public int Id { get; set; }

        [ForeignKey("Usesrs")]
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public List<Products> ProductsList { get; set; } 

        public double TotalPrice { get; set; }

        [Required]
        [Display(Name = "Shipping Address")]
        public String ShippingAdress { get; set; }

        [ForeignKey("ShopingCart")]
        public string ShoppingCartId { get; set; }
    }
}