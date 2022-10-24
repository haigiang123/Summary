using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Summary.WebApi.Models
{
    public class LoginVM
    {
        [DefaultValue("admin")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}