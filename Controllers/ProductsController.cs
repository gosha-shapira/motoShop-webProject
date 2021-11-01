using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using motoShop.Data;
using motoShop.Models;
using motoShop.Views.Home;

namespace motoShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly motoShopContext _context;

        public ProductsController(motoShopContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }


        public IActionResult SearchMotorcycle(string searchTerm)
        {
            var query = from p in _context.Motorcycle
                        where p.Model.Contains(searchTerm)
                        select p;

            if (!query.Any())
            {
                return View("NotFound");
            }

            else
            {
                return View("List", new ProductList
                {
                    Products = query

                });
            }
        }


        public IActionResult SearchByType(string typeId)
        {

            ProductType pType = (ProductType)Enum.Parse(typeof(ProductType), typeId);

            var query = from p in _context.Products
                        where p.Type.Equals(pType)
                        select p;

            if (!query.Any() || string.IsNullOrEmpty(typeId))
            {
                return View("NotFound");
            }
            else
            {



                return View("SearchIndex", new ProductList
                {
                    Products = query

                });
            }

            // return View(await _context.Motorcycle.OrderByDescending(a => a.EntryDate).Include(m => m.Branch).ToListAsync());
        }


        // used for grouping by manufacturer the motorcycle search result 
        //public async Task<IActionResult> GroupByManu(string manufacturer)
        //{

        //    var query = from p in _context.Products
        //                where p.Manufacturer.Equals(manufacturer)
        //                select p;

        //    if (!query.Any() || manufacturer == null)
        //    {
        //        return View("NotFound");
        //    }
        //    else
        //    {



        //        return View("SearchIndex", new ProductList
        //        {
        //            Products = query

        //        });

        //    }

        //// used for grouping clothing by subType in search result 
        //public async Task<IActionResult> GroupBySubType(string subType)
        //{
        //        var query = from p in _context.Products
        //                    where p.
        //                    select p;

        //        if (!query.Any() || manufacturer == null)
        //        {
        //            return View("NotFound");
        //        }
        //        else
        //        {



        //            return View("SearchIndex", new ProductList
        //            {
        //                Products = query

        //            });



        //        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Manufacturer,Type,Price,Description,UnitsSold,EntryDate,Sale,Stock")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Manufacturer,Type,Price,Description,UnitsSold,EntryDate,Sale,Stock")] Products products)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
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
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
