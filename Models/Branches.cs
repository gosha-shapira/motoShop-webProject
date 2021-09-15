using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

    public class Branches
    {
        [Key]
        public int ID { get; set; }
        
        public String Adress { get; set; }

    }
