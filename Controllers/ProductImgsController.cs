using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using motoShop.Data;

namespace motoShop.Controllers
{
    public class ProductImgsController : Controller
    {
        private readonly motoShopContext _context;

        public ProductImgsController(motoShopContext context)
        {
            _context = context;
        }

        // GET: ProductImgs
        public async Task<IActionResult> Index()
        {
            var motoShopContext = _context.ProductImg.Include(p => p.Product);
            return View(await motoShopContext.ToListAsync());
        }

        // GET: ProductImgs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImg = await _context.ProductImg
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImg == null)
            {
                return NotFound();
            }

            return View(productImg);
        }

        // GET: ProductImgs/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Discriminator");
            return View();
        }

        // POST: ProductImgs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,ProductId")] ProductImg productImg)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productImg);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Discriminator", productImg.ProductId);
            return View(productImg);
        }

        // GET: ProductImgs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImg = await _context.ProductImg.FindAsync(id);
            if (productImg == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Discriminator", productImg.ProductId);
            return View(productImg);
        }

        // POST: ProductImgs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,ProductId")] ProductImg productImg)
        {
            if (id != productImg.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productImg);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductImgExists(productImg.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Discriminator", productImg.ProductId);
            return View(productImg);
        }

        // GET: ProductImgs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImg = await _context.ProductImg
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImg == null)
            {
                return NotFound();
            }

            return View(productImg);
        }

        // POST: ProductImgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productImg = await _context.ProductImg.FindAsync(id);
            _context.ProductImg.Remove(productImg);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductImgExists(int id)
        {
            return _context.ProductImg.Any(e => e.Id == id);
        }
    }
}
