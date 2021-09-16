using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace motoShop.Models
{
    public enum UserType
    {
        Client,
        Employee,
        Admin
    }
    public class Users
    {
        public UserType Type { get; set; } = UserType.Client;

        [Key]
        [Required]
        public String Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Address { get; set; }
    }
}