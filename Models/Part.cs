using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

    public class Part : Products
    {
        public int Stock { get; set; }

        public int MotorcycleId { get; set; }

        public List<Motorcycle> Compatibility { get; set; }

    }
