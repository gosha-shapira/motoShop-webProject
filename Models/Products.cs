using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

    public class Products
    {
        public int Id { get; set; }

        public String Manufacturer { get; set; }

        public String Type { get; set; }

        public double Price { get; set; }

        public String Description { get; set; }

        public List<ProductImg> Photos { get; set; }

        public int UnitsSold { get; set; }

        public DateTime EntryDate { get; set; }

        public int Sale { get; set; }

    }
