using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Summary.WebApi.Models
{
    public class AddPhoneNumberVM
    {
        [Required]
        [Phone]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}