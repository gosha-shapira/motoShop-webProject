﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

    public class Clothing : Products
    {
        public String Gender { get; set; }

        public int Size { get; set; }

    }
