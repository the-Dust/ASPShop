using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Login
{
    public class LoginUser
    {
        [Required(ErrorMessage ="Required field")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Password { get; set; }
    }
}