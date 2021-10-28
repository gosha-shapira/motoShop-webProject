using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using motoShop.Data;
using motoShop.Models;

namespace motoShop.Controllers
{
    public class PartsController : Controller
    {
        private readonly motoShopContext _context;

        public PartsController(motoShopContext context)
        {
            _context = context;
        }

        // GET: Parts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Part.Include(m => m.Branch).ToListAsync());
        }

        // GET: Parts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Part
                .Include(x => x.Compatibility)
                .Include(m => m.Branch).Include(x => x.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
            Motorcycle m = _context.Motorcycle.Single(x => x.Id == part.MotorcycleId);
            part.Compatibility = part.Compatibility.Append<Motorcycle>(m);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // GET: Parts/Create
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            ViewData["MotorcycleId"] = new SelectList(_context.Motorcycle, "Id", "Description");
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "Address");
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MotorcycleId,Id,Manufacturer,Price,Description,UnitsSold,Sale,Stock,BranchId,SubType")] Part part)
        {
            if (ModelState.IsValid)
            {
                part.Photos = new List<ProductImg>();
                part.Compatibility = new List<Motorcycle>();
                part.EntryDate = DateTime.Now;
                part.Type = ProductType.Part;
                Motorcycle m = _context.Motorcycle.Single(x => x.Id == part.MotorcycleId);
                part.Compatibility = part.Compatibility.Append<Motorcycle>(m);
                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "Address", part.BranchId);
            return View(part);
        }

        // GET: Parts/Edit/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Part.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "Address", part.BranchId);
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MotorcycleId,Id,Manufacturer,Price,Description,UnitsSold,EntryDate,Sale,Stock,BranchId,SubType")] Part part)
        {
            if (id != part.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    part.Type = ProductType.Part;
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartExists(part.Id))
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
            return View(part);
        }

        // GET: Parts/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var part = await _context.Part
                .Include(m => m.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var part = await _context.Part.FindAsync(id);
            _context.Part.Remove(part);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartExists(int id)
        {
            return _context.Part.Any(e => e.Id == id);
        }
    }
}
