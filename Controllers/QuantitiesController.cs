using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using motoShop.Data;
using motoShop.Models;

namespace motoShop.Controllers
{
    public class QuantitiesController : Controller
    {
        private readonly motoShopContext _context;

        public QuantitiesController(motoShopContext context)
        {
            _context = context;
        }

        // GET: Quantities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Quantity.ToListAsync());
        }

        // GET: Quantities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quantity = await _context.Quantity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quantity == null)
            {
                return NotFound();
            }

            return View(quantity);
        }

        // GET: Quantities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quantities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Amount")] Quantity quantity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quantity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quantity);
        }

        // GET: Quantities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quantity = await _context.Quantity.FindAsync(id);
            if (quantity == null)
            {
                return NotFound();
            }
            return View(quantity);
        }

        // POST: Quantities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Amount")] Quantity quantity)
        {
            if (id != quantity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quantity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuantityExists(quantity.Id))
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
            return View(quantity);
        }

        // GET: Quantities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quantity = await _context.Quantity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quantity == null)
            {
                return NotFound();
            }

            return View(quantity);
        }

        // POST: Quantities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quantity = await _context.Quantity.FindAsync(id);
            _context.Quantity.Remove(quantity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuantityExists(int id)
        {
            return _context.Quantity.Any(e => e.Id == id);
        }
    }
}
