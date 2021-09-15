using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class Part : Products
    {

        public int MotorcycleId { get; set; }

        public IEnumerable<Motorcycle> Compatibility { get; set; }

    }
}