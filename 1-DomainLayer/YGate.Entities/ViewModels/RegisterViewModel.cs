using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YGate.Entities.ViewModels
{
    public class RegisterViewModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? RPassword { get; set; }
        public string? Email { get; set; }


        public string Controll() {
            if (string.IsNullOrEmpty(UserName))
                return "Please provide User name";

            if (string.IsNullOrEmpty(Password))
                return "Please provide Password";

            if (string.IsNullOrEmpty(RPassword))
                return "Please provide Retry Password";

            if (string.IsNullOrEmpty(Email))
                return "Please provide retry Email";

            if (Password.ToString() != RPassword.ToString())
               return "Password did not match";

            if (!Regex.IsMatch(Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
               return "Invalid email";

            return "";
        }

    }
}
