using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public class ProductImg
    {
        [Key]
        public int Id { get; set; }

        public byte[] Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public int ProductId { get; set; }

        public Products Product { get; set; }
    }
}