using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities.ViewModels
{
    public class LoginViewModel
    {

        public LoginViewModel()
        {
            UserName = "";
            Password = "";
        }

        [Required(AllowEmptyStrings = false,ErrorMessage ="Please provide User name")]
        public string? UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide Password")]
        public string? Password { get; set; }
    }
}
