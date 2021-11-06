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
    public class ClothingsController : Controller
    {
        private readonly motoShopContext _context;

        public ClothingsController(motoShopContext context)
        {
            _context = context;
        }

        // GET: Clothings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clothing.Include(m => m.Branch).Include(m => m.Photos).ToListAsync());
        }

        // GET: Clothings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothing = await _context.Clothing
                .Include(m => m.Branch).Include(x => x.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clothing == null)
            {
                return NotFound();
            }

            return View(clothing);
        }

        // GET: Clothings/Create
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "BranchName");
            return View();
        }

        // POST: Clothings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Gender,Size,Id,Manufacturer,Price,Description,UnitsSold,Sale,Stock,BranchId,SubType")] Clothing clothing)
        {
            if (ModelState.IsValid)
            {
                clothing.Photos = new List<ProductImg>();
                clothing.EntryDate = DateTime.Now;
                clothing.Type = ProductType.Clothing;
                _context.Add(clothing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "BranchName", clothing.BranchId);
            return View(clothing);
        }

        // GET: Clothings/Edit/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothing = await _context.Clothing.FindAsync(id);
            if (clothing == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "BranchName", clothing.BranchId);
            return View(clothing);
        }

        // POST: Clothings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Gender,Size,Id,Manufacturer,Price,Description,UnitsSold,EntryDate,Sale,Stock,BranchId,SubType")] Clothing clothing)
        {
            if (id != clothing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    clothing.Type = ProductType.Clothing;
                    _context.Update(clothing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClothingExists(clothing.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "BranchName", clothing.BranchId);
            return View(clothing);
        }

        // GET: Clothings/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothing = await _context.Clothing
                .Include(m => m.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clothing == null)
            {
                return NotFound();
            }

            return View(clothing);
        }

        // POST: Clothings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clothing = await _context.Clothing.FindAsync(id);
            _context.Clothing.Remove(clothing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClothingExists(int id)
        {
            return _context.Clothing.Any(e => e.Id == id);
        }
    }
}
