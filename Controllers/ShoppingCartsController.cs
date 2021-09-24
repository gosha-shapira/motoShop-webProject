using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using motoShop.Data;
using motoShop.Models;
using motoShop.Views.ShoppingCarts;

namespace motoShop.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly motoShopContext _context;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartsController(motoShopContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            // Get from data base and View All the Products of the current Cart
            _shoppingCart.Items = (_shoppingCart.Items ??= await _context.ShoppingCartItems
                .Where(c => c.ShoppingCartId.Equals(_shoppingCart.ShoppingCartId))
                .Include(s => s.Product)
                .ToListAsync());

            var shoppingCartViewModel = new ShoppingCartView
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = await GetShoppingCartITotal()
            };

            return View(shoppingCartViewModel);

        }



        public async Task<RedirectToActionResult> RemoveFromShoppingCart(int id)
        {
            var selectedProduct = GetAllProducts.FirstOrDefault(c => c.Id == id);

            if (selectedProduct != null)
            {
                await RemoveFromCart(selectedProduct);
            }

            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> AddToShoppingCart(int id)
        {
            var selectedProduct = GetProduct(id);

            if (selectedProduct != null)
            {
                AddToCart(selectedProduct, 1);
            }

            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> ClearFromShoppingCart(int id)
        {
            var selectedProduct = GetAllProducts.FirstOrDefault(c => c.Id == id);

            if (selectedProduct != null)
            {
                await ClearFromCart(selectedProduct);
            }

            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> ClearCart()
        {
            var cartItems = _context.ShoppingCartItems.Where(c => c.ShoppingCartId == _shoppingCart.ShoppingCartId);

            _context.ShoppingCartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Helpers
        public IEnumerable<Products> GetAllProducts
        {
            get
            {
                return _context.Products;
            }
        }

        // Helpers
        public Products GetProduct(int id)
        {
            
                return _context.Products.Find(id);
        }


        // Add a product to the Cart and Save it (the item) in the Data Base, ShoppingCartItems table
        public void AddToCart(Products prod, int quantity)
        {

            var shoppingCartItem = _context.ShoppingCartItems.SingleOrDefault
                (s => s.Product.Id == prod.Id && s.ShoppingCartId == _shoppingCart.ShoppingCartId);



            // If the condition is true: We are creating a new instance of ShoppingCartItem with the Product's data (not instance of Shopping Cart)
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = _shoppingCart.ShoppingCartId,
                    Product = prod,
                    Quantity = quantity
                };

                // Add this new Item into the same Shopping Cart
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            // Else if the Item allready exists in the same Shopping Cart
            {
                shoppingCartItem.Quantity++;
            }
            _context.SaveChanges();
        }


        //Remove an Item (Product) From the Cart and Save Changes in the Data Base, ShoppingCartItems table
        public async Task<int> RemoveFromCart(Products prod)
        {
            var shoppingCartItem = await _context.ShoppingCartItems.SingleOrDefaultAsync
                (s => s.Product.Id == prod.Id && s.ShoppingCartId == _shoppingCart.ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Quantity > 1)
                {
                    shoppingCartItem.Quantity--;
                    localAmount = shoppingCartItem.Quantity;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            await _context.SaveChangesAsync();

            return localAmount;
        }

        //Adds an Item (Product) From the Cart and Save Changes in the Data Base, ShoppingCartItems table
        /*public async Task<int> AddToCart(Products prod)
        {
            var shoppingCartItem = await _context.ShoppingCartItems.SingleOrDefaultAsync
                (s => s.Product.Id == prod.Id && s.ShoppingCartId == _shoppingCart.ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem.Quantity >= 1)
            {
                shoppingCartItem.Quantity++;
                localAmount = shoppingCartItem.Quantity;
            }

            await _context.SaveChangesAsync();
            return localAmount;
        }*/

        //Clears an Item (Product) From the Cart and Save Changes in the Data Base, ShoppingCartItems table
        public async Task<int> ClearFromCart(Products prod)
        {
            var shoppingCartItem = await _context.ShoppingCartItems.SingleOrDefaultAsync
                (s => s.Product.Id == prod.Id && s.ShoppingCartId == _shoppingCart.ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                _context.ShoppingCartItems.Remove(shoppingCartItem);
            }

            await _context.SaveChangesAsync();

            return localAmount;
        }

        public IActionResult ShoppingCartSummary(int prodId)
        {
            var selectedProduct = GetAllProducts.FirstOrDefault(c => c.Id == prodId);

            if (selectedProduct != null)
            {
                AddToCart(selectedProduct, 1);
            }

            return ViewComponent("ShoppingCartSummary");
        }

        public async Task<double> GetShoppingCartITotal()
        {
            var totalPrice = await _context.ShoppingCartItems.Where(c => c.ShoppingCartId == _shoppingCart.ShoppingCartId).Select(c => c.Product.Price * c.Quantity).SumAsync();
            return totalPrice;
        }
    }
}
