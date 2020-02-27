using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class Register
    {
        public int Id { get; set; }
        [Required, MaxLength(256)]
        public string Username { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [Required, MaxLength(256)]
        public string Surname { get; set; }

        [Required, MaxLength(256)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }


        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
        [Display(Name = "Remember Me")]

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
