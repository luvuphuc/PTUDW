using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectASP.Areas.Admin.Model
{
    public class LoginModel
    {
        [Required]
        public string Username { set; get; }
        public string Password { set; get; }
        public bool RememberMe { set; get; }

    }
}