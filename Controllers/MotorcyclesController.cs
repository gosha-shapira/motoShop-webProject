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
    public class MotorcyclesController : Controller
    {
        private readonly motoShopContextLocal _context;

        public MotorcyclesController(motoShopContextLocal context)
        {
            _context = context;
        }

        // GET: Motorcycles
        public async Task<IActionResult> Index()
        {
            // GET THE LIST OF THE BRANCHES
            var branches = _context.Branches.ToList();

            MotorCycleViewModel viewModel = new MotorCycleViewModel()
            {
                Branches = branches,
                SelectedBranch = 0,
                Motorcycles = await _context.Motorcycle.ToListAsync()
            };


            return View(viewModel);
        }

        //serach method
        public async Task<IActionResult> Search(string title, int branchId)
        {
            var query = await _context.Motorcycle
                                .Where(k => k.BranchId == branchId) 
                                .ToListAsync();

            // GET THE LIST OF THE BRANCHES
            var branches = _context.Branches.ToList();

            MotorCycleViewModel viewModel = new MotorCycleViewModel()
            {
                Branches = branches,
                SelectedBranch = branchId,
                Motorcycles = query
            };

            return View("Index", viewModel);
        }
        public async Task<IActionResult> SearchJson(string title, int branchId)
        {
            var query = await _context.Motorcycle
                                .Where(k => k.BranchId == branchId)
                                .ToListAsync();

            // GET THE LIST OF THE BRANCHES
            var branches = _context.Branches.ToList();

            MotorCycleViewModel viewModel = new MotorCycleViewModel()
            {
                Branches = branches,
                SelectedBranch = branchId,
                Motorcycles = query
            };



            //return Json(viewModel.Branches.ToList());
            return Json(viewModel.Branches.ToList()); 
        }

        // GET: Motorcycles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorcycle = await _context.Motorcycle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (motorcycle == null)
            {
                return NotFound();
            }

            return View(motorcycle);
        }

        // GET: Motorcycles/Create
        public async Task<IActionResult> Create()
        {
            //return all branches as a list

            ViewData["Branches"] = new SelectList(await _context.Branches.ToListAsync(), nameof(Branches.ID), nameof(Branches.BranchName));

            return View();
        }

        // POST: Motorcycles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Model,Year,EngineSize,LicenseType,Id,Manufacturer,Type,Price,Description,UnitsSold,EntryDate,Sale,Stock")] Motorcycle motorcycle, int Branch)
        {
            if (ModelState.IsValid)
            {
                motorcycle.Branch = await _context.Branches.FirstAsync(x => x.ID == Branch); // returns the first Branch-ID 

                _context.Add(motorcycle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(motorcycle);
        }

        // GET: Motorcycles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorcycle = await _context.Motorcycle.FindAsync(id);
            if (motorcycle == null)
            {
                return NotFound();
            }
            return View(motorcycle);
        }

        // POST: Motorcycles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Model,Year,EngineSize,LicenseType,Id,Manufacturer,Type,Price,Description,UnitsSold,EntryDate,Sale,Stock")] Motorcycle motorcycle)
        {
            if (id != motorcycle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(motorcycle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MotorcycleExists(motorcycle.Id))
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
            return View(motorcycle);
        }

        // GET: Motorcycles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorcycle = await _context.Motorcycle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (motorcycle == null)
            {
                return NotFound();
            }

            return View(motorcycle);
        }

        // POST: Motorcycles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var motorcycle = await _context.Motorcycle.FindAsync(id);
            _context.Motorcycle.Remove(motorcycle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MotorcycleExists(int id)
        {
            return _context.Motorcycle.Any(e => e.Id == id);
        }
    }
}
