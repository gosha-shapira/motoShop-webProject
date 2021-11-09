using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using motoShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using motoShop.Data;
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
            List<String> YouTube = new List<String>();
            YouTube.Add("https://www.youtube.com/embed/hQOYCgTwqpg");
            YouTube.Add("https://www.youtube.com/embed/yXB4eXXD0wI");
            YouTube.Add("https://www.youtube.com/embed/LHTGf7rJlCU");
            YouTube.Add("https://www.youtube.com/embed/1bNpStS0Xsk");
            YouTube.Add("https://www.youtube.com/embed/inzO_V_Erzg");
            Random rnd = new Random();
            int Rand = rnd.Next(5);
            ViewData["YouTube"] = YouTube.ElementAt(Rand);

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
            return View(await _context.Products.Include(m => m.Branch).ToListAsync());
        }
        [HttpPost]
        public IActionResult Search(String Type, String SubType, String Manu, String text)
        {
            Debug.WriteLine("////////////    Tpye=" + Type + " SubType=" + SubType + " Manu=" + Manu+" Text="+text);
            ProductType PType = ProductType.Motorcycle;
            if (text == null)
            {
                if (Type != null)
                {
                    if (Type.Equals("Motorcycle"))
                        PType = ProductType.Motorcycle;
                    if (Type.Equals("Clothing"))
                        PType = ProductType.Clothing;
                    if (Type.Equals("Part"))
                        PType = ProductType.Part;
                    if ((SubType != null) && (Manu != null))
                        return View(_context.Products.Where(x => x.Type == PType && x.SubType.Equals(SubType) && x.Manufacturer.Equals(Manu)).Include(m => m.Branch).Include(m => m.Photos).ToList());
                    if ((SubType != null) && (Manu == null))
                        return View(_context.Products.Where(x => x.Type == PType && x.SubType.Equals(SubType)).Include(m => m.Branch).Include(m => m.Photos).ToList());
                    if ((SubType == null) && (Manu != null))
                        return View(_context.Products.Where(x => x.Type == PType && x.Manufacturer.Equals(Manu)).Include(m => m.Branch).Include(m => m.Photos).ToList());
                    if ((SubType == null) && (Manu == null))
                        return View(_context.Products.Where(x => x.Type == PType).Include(m => m.Branch).Include(m => m.Photos).ToList());
                }
                else
                {
                    if ((SubType != null) && (Manu != null))
                        return View(_context.Products.Where(x => x.SubType.Equals(SubType) && x.Manufacturer.Equals(Manu)).Include(m => m.Branch).Include(m => m.Photos).ToList());
                    if ((SubType != null) && (Manu == null))
                        return View(_context.Products.Where(x => x.SubType.Equals(SubType)).Include(m => m.Branch).Include(m => m.Photos).ToList());
                    if ((SubType == null) && (Manu != null))
                        return View(_context.Products.Where(x => x.Manufacturer.Equals(Manu)).Include(m => m.Branch).Include(m => m.Photos).ToList());
                    if ((SubType == null) && (Manu == null))
                        return View(_context.Products.Include(m => m.Branch).Include(m => m.Photos).ToList());
                }
            }
            else
            {
                return View(_context.Products.Where(x => x.Description.Contains(text)).Include(m => m.Branch).Include(m => m.Photos).ToList());
            }
            return View();

        }
    }
}
