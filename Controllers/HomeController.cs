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
using System.Web;

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
            List<String> Types = new List<String>();
            List<String> SubTypes = new List<String>();
            List<String> Manufacturers = new List<String>();

            foreach (Products row in _context.Products)
            {
                Types.Add(row.Type.ToString());
                SubTypes.Add(row.SubType.ToString());
                Manufacturers.Add(row.Manufacturer.ToString());
            }

            Types = Types.Distinct().ToList();
            SubTypes = SubTypes.Distinct().ToList();
            Manufacturers = Manufacturers.Distinct().ToList();

            ViewData["Type"] = new SelectList(Types);
            ViewData["SubType"] = new SelectList(SubTypes);
            ViewData["Manufacturer"] = new SelectList(Manufacturers);

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
        [HttpPost]
        public IActionResult Search(String Type,String SubType, String Manu)
        {
            Debug.WriteLine("////////////    Tpye="+Type+" SubType="+SubType+" Manu="+Manu);
            ProductType PType = ProductType.Motorcycle;
            if (Type != null)
            {
                if (Type.Equals("Motorcycle"))
                    PType = ProductType.Motorcycle;
                if (Type.Equals("Clothing"))
                    PType = ProductType.Clothing;
                if (Type.Equals("Part"))
                    PType = ProductType.Part;
            }
            return View(_context.Products.Where(x => x.Type == PType && x.SubType.Equals(SubType) && x.Manufacturer.Equals(Manu)).Include(m => m.Branch).ToList());
        }
    }
}
