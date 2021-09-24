using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class Motorcycle : Products
    {
        public String Model { get; set; }

        public int Year { get; set; }
        [Display(Name="Engine Size")]
        public double EngineSize { get; set; }
        [Display(Name = "License Type")]
        public String LicenseType { get; set; }

    }
}