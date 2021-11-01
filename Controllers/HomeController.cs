using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using motoShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using motoShop.Data;
using motoShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace motoShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly motoShopContext _context;

        public HomeController(ILogger<HomeController> logger, motoShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //var Types = from m in _context.Products
            //            group m by m.Type into grp
            //            select new { Type = grp.Key};
            var Types = ((from m in _context.Products
                        select m).Distinct()).ToList();

            ViewData["Type"] = new SelectList(_context.Products, "Id", "Type");
            ViewData["SubType"] = new SelectList(_context.Products, "Id", "SubType");
            ViewData["Manufacturer"] = new SelectList(_context.Products, "Id", "Manufacturer");
            HomeIndexModel HomeContext = new HomeIndexModel();
            HomeContext.Products = _context.Products.OrderByDescending(a => a.EntryDate).Include(x => x.Photos).ToList<Products>();
            HomeContext.Branches = _context.Branches.ToList<Branches>();

            return View(HomeContext);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> SearchAsync()
        {
            //HomeIndexModel HomeContext = new HomeIndexModel();
            //HomeContext.Products = _context.Products.OrderByDescending(a => a.EntryDate).Include(x => x.Photos).ToList<Products>();
            //HomeContext.Branches = _context.Branches.ToList<Branches>();
            //return View(HomeContext);
            return View(await _context.Products.Include(m => m.Branch).ToListAsync());
        }
    }
}
