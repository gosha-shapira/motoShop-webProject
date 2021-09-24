using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using motoShop.Data;
using motoShop.Models;
using motoShop.Views.ShoppingCarts;
using System.Linq;

namespace motoShop.Components
{

    [ViewComponent(Name = "ShoppingCartSummary")]
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly motoShopContext _context;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartSummary(motoShopContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            _shoppingCart.Items = (_shoppingCart.Items ??= _context.ShoppingCartItems.Where
                (c => c.ShoppingCartId.Equals(_shoppingCart.ShoppingCartId)).Include(s => s.Product).ToList());

            
            var total = _context.ShoppingCartItems.Where(c => c.ShoppingCartId.Equals(_shoppingCart.ShoppingCartId))
                .Select(c => c.Product.Price * c.Quantity).Sum();

            var shoppingCartViewModel = new ShoppingCartView
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = total
            };

            return View(shoppingCartViewModel);
        }
    }
}
