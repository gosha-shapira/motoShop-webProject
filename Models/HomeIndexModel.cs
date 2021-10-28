using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class HomeIndexModel
    {
        public List<Products> Products { get; set; }
        public List<Branches> Branches { get; set; }
    }
}
