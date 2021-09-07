using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using motoShop.Models;

namespace motoShop.Data
{
    public class motoShopContext : DbContext
    {
        public motoShopContext (DbContextOptions<motoShopContext> options)
            : base(options)
        {
        }

        public DbSet<motoShop.Models.Product> Product { get; set; }
    }
}
