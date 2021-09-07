using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace motoShop.Data
{
    public class motoShopContext : DbContext
    {
        public motoShopContext (DbContextOptions<motoShopContext> options)
            : base(options)
        {
        }

        public DbSet<Products> Products { get; set; }
    }
}
