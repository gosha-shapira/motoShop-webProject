using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using motoShop.Data;
using motoShop.Models;

namespace motoShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly motoShopContext _context;
        private readonly ShoppingCart _shoppingCart;

        public OrdersController(motoShopContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        // GET: Orders
        [Authorize]
        public IActionResult Checkout()
        {

            ViewData["User"] = User.Identity.Name;

            return View();
        }

        /* // GET: Orders/Details/5 //<------ show all the Items for this order
         public async Task<IActionResult> Details(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var order = await _context.Order
                 .FirstOrDefaultAsync(m => m.OrderId == id);
             if (order == null)
             {
                 return NotFound();
             }

             return View(order);
         }*/

        /*// GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }*/

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("OrderId,UserId,TotalPrice")] Order order, string userId)
        {
            _shoppingCart.Items = GetShoppingCartItems();

            if (_shoppingCart.Items.Count == 0)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                CreateOrder(order);
                var shopCart = _context.ShoppingCart.Find(_shoppingCart.ShoppingCartId);
                shopCart.OrderId = (from orders in _context.Order
                                    orderby orders.Id descending
                                    select orders.Id).First();
                _context.SaveChanges();
                return RedirectToAction("CheckoutComplete", order);
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,BuyerId,TotalPrice,ShippingAdress")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }


        // ------------------------------ Private Functions ------------------------------

        private List<ShoppingCartItem> GetShoppingCartItems()
        {

            return _shoppingCart.Items = _shoppingCart.Items ?? (_shoppingCart.Items = _context.ShoppingCartItems
                .Where(c => c.ShoppingCartId == _shoppingCart.ShoppingCartId)
                .Include(p => p.Product)
                .ToList());
        }

        private void CreateOrder(Order order)
        {
            var usr = _context.Users.Find(User.Identity.Name);

            order.UserId = User.Identity.Name;
            order.ShippingAdress = usr.Address;
            order.OrderDate = DateTime.Now;
            order.TotalPrice = GetShoppingCartITotal();
            order.ShoppingCartId = _shoppingCart.ShoppingCartId;
            _context.Order.Add(order);
            /*var shopCart = _context.ShoppingCart.Find(_shoppingCart.ShoppingCartId);
            shopCart.OrderId = (from orders in _context.Order
                                orderby orders.Id descending
                                select orders.Id).First();*/
            _context.SaveChanges();
        }

        private double GetShoppingCartITotal()
        {
            var total = _context.ShoppingCartItems
                .Where(c => c.ShoppingCartId == _shoppingCart.ShoppingCartId)
                .Select(c => c.Product.Price * c.Quantity)
                .Sum();
            return total;
        }

        private void ClearCart(Order order)
        {
            var cartItems = GetShoppingCartItems();

            foreach (var item in cartItems) // stock updating
            {
                var resProd = _context.Products.Find(item.Product.Id);
                resProd.Stock -= item.Quantity;
            }

            _context.SaveChanges();
        }

        public IActionResult CheckoutComplete(Order order)
        {
            ClearCart(order); 
            ViewBag.CheckoutCompleteMessage = "Thank you for your order.";
            ViewBag.OrderNumber = order.Id;
            HttpContext.Session.Clear();
            return View();
        }

        // returns view of all orders mase by the user
        public IActionResult OrderHistory(string? username)
        {
            var order = from o in _context.Order
                        join u in _context.Users
                        on o.UserId equals u.Username
                        where o.UserId == username
                        select o;


            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
