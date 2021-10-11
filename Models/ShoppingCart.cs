using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using motoShop.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class ShoppingCart
    {
        private readonly motoShopContext _motoShopContext;

        [Key]
        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> Items { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public ShoppingCart(motoShopContext motoShopContext)
        {
            _motoShopContext = motoShopContext;
        }


        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<motoShopContext>();
            
            string cartId = session.GetString("CartId")??Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

    }
}
