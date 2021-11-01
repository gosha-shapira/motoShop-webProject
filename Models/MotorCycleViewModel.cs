using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class MotorCycleViewModel
    {
        public List<Branches> Branches { get; set; }
        public int SelectedBranch { get; set; }
        public IEnumerable<Motorcycle> Motorcycles { get; set; }
        //public IEnumerable<Part> Parts { get; set; }
        //public IEnumerable<Clothing> Clothes { get; set; }
    }
}
