using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kortyérzet.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid E-mail format.")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
