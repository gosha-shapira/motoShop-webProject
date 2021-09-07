using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

    public class ProductImg
    {
        public int Id { get; set; }

        public byte[] Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public int ProductId { get; set; }

        public Products Product { get; set; }
    }
