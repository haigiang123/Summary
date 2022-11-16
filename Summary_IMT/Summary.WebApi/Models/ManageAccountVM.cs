using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Summary.WebApi.Models
{
    public class ManageAccountVM
    {
        public string AccountId { get; set; }
        //public string HashPassword { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        //public bool HasPassword
        //{
        //    get
        //    {
        //        return !string.IsNullOrEmpty(HashPassword) ? true : false;
        //    }
        //}
        public bool HasPassword { get; set; }
    }
}