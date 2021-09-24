using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }

        public String Manufacturer { get; set; }

        public String Type { get; set; }
        [Required]
        public double Price { get; set; }

        public String Description { get; set; }

        public IEnumerable<ProductImg> Photos { get; set; } = new List<ProductImg>();
        [Display(Name = "Units Sold")]
        public int UnitsSold { get; set; }
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; } = DateTime.Now;

        public int Sale { get; set; }

        public int Stock { get; set; }

    }
}