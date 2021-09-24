using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class Motorcycle : Products
    {
        public String Model { get; set; }

        public int Year { get; set; }

        public double EngineSize { get; set; }

        public String LicenseType { get; set; }

        public string SubType { get; set; }


    }
}