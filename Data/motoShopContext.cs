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

        public DbSet<Products> Products { get; set; }

        public DbSet<Branches> Branches { get; set; }

        public DbSet<Motorcycle> Motorcycle { get; set; }

        public DbSet<Clothing> Clothing { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<Part> Part { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<ProductImg> ProductImg { get; set; }

        public DbSet<Quantity> Quantity { get; set; }
    }
}
