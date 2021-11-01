﻿using System;
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
    public class MotorcyclesController : Controller
    {
        private readonly motoShopContext _context;

        public MotorcyclesController(motoShopContext context)
        {
            _context = context;
        }

        // GET: Motorcycles
        public async Task<IActionResult> Index(string Sorting_Order)
        {
            Console.Out.WriteLine(Sorting_Order);
            //ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Manufacturer" : "";
            //ViewBag.SortingDate = Sorting_Order == "Date_Up" ? "Date_Up" : "Date_Down";
            switch (Sorting_Order)
            {
                case "Man_Up":
                    return View(await _context.Motorcycle.OrderBy(a => a.Manufacturer).Include(m => m.Branch).ToListAsync());
                case "Man_Down":
                    return View(await _context.Motorcycle.OrderByDescending(a => a.Manufacturer).Include(m => m.Branch).ToListAsync());
                case "Date_Up":
                    return View(await _context.Motorcycle.OrderByDescending(a => a.EntryDate).Include(m => m.Branch).ToListAsync());
                case "Date_Down":
                    return View(await _context.Motorcycle.OrderBy(a => a.EntryDate).Include(m => m.Branch).ToListAsync());
                case "Year_Up":
                    return View(await _context.Motorcycle.OrderBy(a => a.Year).Include(m => m.Branch).ToListAsync());
                case "Year_Down":
                    return View(await _context.Motorcycle.OrderByDescending(a => a.Year).Include(m => m.Branch).ToListAsync());
                case "Engine_Up":
                    return View(await _context.Motorcycle.OrderBy(a => a.EngineSize).Include(m => m.Branch).ToListAsync());
                case "Engine_Down":
                    return View(await _context.Motorcycle.OrderByDescending(a => a.EngineSize).Include(m => m.Branch).ToListAsync());
                case "Price_Up":
                    return View(await _context.Motorcycle.OrderBy(a => a.Price).Include(m => m.Branch).ToListAsync());
                case "Price_Down":
                    return View(await _context.Motorcycle.OrderByDescending(a => a.Price).Include(m => m.Branch).ToListAsync());
                default:
                    return View(await _context.Motorcycle.OrderByDescending(a => a.EntryDate).Include(m => m.Branch).ToListAsync());
            }
            /*return View(await _context.Motorcycle.ToListAsync());
            var motoShopContext = _context.Motorcycle.Include(m => m.Branch);
            return View(await motoShopContext.ToListAsync());*/
        }


        // connecting to twitter api
        public static async Task<String> PostMessageToTwitter(string description)
        {
            string ConsumerKey = "WAzkTTosR6FdgkeShYiHuUpwS";
            string ConsumerKeySecret = "MIX118Byu1l1wmKnTvKr2f5Y5NMrwVx6UOdkmvJDIPE1qc250i";
            string AccessToken = "1383107089816518666-ekU8DOS6z9vVfRs5AE0x2I0MCCQPxP";
            string AccessTokenSecret = "uxsyBJ9anTcn2wAQw2yqvDS2ckfS3uvjLzO9mgDiwhfeU";

            var twitter = new TwitterAPI(ConsumerKey,
                ConsumerKeySecret, AccessToken, AccessTokenSecret);

            string message = "A new motorcycle has been added to our store, we are proud to introduce the new " +
                  description +  ", come check it out!";

            var response = await twitter.Tweet(message);
            Console.WriteLine(response);

            return response;
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

            
            return View("Search", viewModel);
        }


        //search Json
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



            return Json(viewModel.Branches.ToList());

        }



        // GET: Motorcycles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var motorcycle = await _context.Motorcycle.Include(x => x.Photos);
            var motorcycle = await _context.Motorcycle
                .Include(m => m.Branch).Include(x => x.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (motorcycle == null)
            {
                return NotFound();
            }

            return View(motorcycle);
        }

        // GET: Motorcycles/Create
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "Address");
            return View();
        }

        // POST: Motorcycles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Model,Year,EngineSize,LicenseType,SubType,Id,Manufacturer,Type,Price,Description,UnitsSold,EntryDate,Sale,Stock,BranchId")] Motorcycle motorcycle)
        {

            if (ModelState.IsValid)
            {
                motorcycle.Photos = new List<ProductImg>();
                motorcycle.EntryDate = DateTime.Now;
                _context.Add(motorcycle);
                await _context.SaveChangesAsync();
                PostMessageToTwitter(motorcycle.Description).Wait(); // use Twitter API when a new motorcycle is added
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "Address", motorcycle.BranchId);
            return View(motorcycle);
        }

        // GET: Motorcycles/Edit/5
        [Authorize(Roles = "Admin,Employee")]
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "Address", motorcycle.BranchId);
            return View(motorcycle);
        }

        // POST: Motorcycles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Model,Year,EngineSize,LicenseType,SubType,Id,Manufacturer,Type,Price,Description,UnitsSold,EntryDate,Sale,Stock,BranchId")] Motorcycle motorcycle)
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "ID", "Address", motorcycle.BranchId);
            return View(motorcycle);
        }

        // GET: Motorcycles/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motorcycle = await _context.Motorcycle
                .Include(m => m.Branch)
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
