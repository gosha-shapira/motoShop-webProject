using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using motoShop.Data;
using motoShop.Models;

namespace motoShop.Controllers
{
    
    [Authorize(Roles = "Admin")] // <-----------------------
    public class UsersController : Controller
    {
        private readonly motoShopContext _context;

        public UsersController(motoShopContext context)
        {
            _context = context;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Users.ToListAsync());
            }
            return RedirectToAction(nameof(AccessDenied));
        }
        
        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? username)
        {
            if (username == null)
            {
                return View("Error");
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Username == username);
            if (users == null)
            {
                return View("Error");
            }

            return View(users);
        }

        private async void loginUser(string username, UserType type)
        {
            HttpContext.Session.SetString("username", username);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, type.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        // GET: Users Registration
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users Registration
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password,FirstName,LastName,Address")] Users users)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.FirstOrDefault(u => u.Username == users.Username) != null)
                {
                    ViewData["Error"] = "Cannot creat username";
                    return View(users);
                }
                _context.Add(users);
                await _context.SaveChangesAsync();

                loginUser(users.Username, UserType.Client);
                return Redirect("~/Home/Index");
            }
            return View(users);
        }

        // GET: Users Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] Users users)
        {
            if (users.Username != null && users.Password != null)
            {
                var q = from u in _context.Users
                        where u.Username == users.Username && u.Password == users.Password
                        select u;

                //var qu = _context.Users.First(u => u.Username == users.Username);

                if (!q.Any())
                {
                    ViewData["Error"] = "Invalid username/password";
                }
                else
                {
                    loginUser(q.First().Username, q.First().Type);
                    //return RedirectToAction(nameof(Register));
                    //return RedirectToAction("Index");
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return Redirect("~/Home/Index");
                    }
                }
            }
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? username)
        {
            if (username == null)
            {
                return View("Error");
            }

            var users = await _context.Users.FindAsync(username);

            if (users == null)
            {
                return View("Error");
            }

            //ViewData["Type"] = new SelectList(_context.Set<Users>(), "Type", "Type", users.Type);
            ViewBag.Type = new SelectList(Enum.GetNames(typeof(UserType)).ToList());
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string username, [Bind("Type,Username,Password,FirstName,LastName,Adress")] Users users)
        {
            if (username != users.Username)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Username))
                    {
                        return View("Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string? username)
        {
            if (username == null)
            {
                return View("Error");
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Username == username);
            if (users == null)
            {
                return View("Error");
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string username)
        {
            var users = await _context.Users.FindAsync(username);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(string username)
        {
            return _context.Users.Any(e => e.Username == username);
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
