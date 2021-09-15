using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

    public class Users
    {
        [Key]
        public int Id { get; set; }

        public String Type { get; set; }
        [Required]
        public String Username { get; set; }
        [Required]
        public String Password { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Adress { get; set; }
    }
