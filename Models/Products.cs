using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{

    public enum ProductType
    {
        Motorcycle = 1,
        Part = 2,
        Clothing = 3
    }
    public class Products
    {
        [Key]
        public int Id { get; set; }

        public String Manufacturer { get; set; }

        [Required]

        public ProductType Type { get; set; } // changed from String  to Enum

        public double Price { get; set; }

        public String Description { get; set; }

        public IEnumerable<ProductImg> Photos { get; set; } = new List<ProductImg>();
        [Display(Name = "Units Sold")]
        public int UnitsSold { get; set; }
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; } = DateTime.Now;

        public int Sale { get; set; }

        public int Stock { get; set; }

        public Branches Branch { get; set; }
        public int BranchId { get; set; } // check Controller?




    }
}