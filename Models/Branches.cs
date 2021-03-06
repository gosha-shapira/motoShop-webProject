using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class Branches
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public String Address { get; set; }
        
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }
        
        [Required]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [Required]
        [RegularExpression(@"^\(?(([0-9]{2})|([0-9]{3}))\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "The number you entered is not valid")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public IEnumerable<Products> Products { get; set; }


    }
}
