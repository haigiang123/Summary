using Summary.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Summary.WebApi.Models
{
    public class IntegrateTinyMCEVM
    {
        [Key]
        public int NewsID { get; set; }

        public string Title { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public HttpPostedFileBase[] Image { get; set; }

        #region User info

        public string UserId { get; set; }

        public List<AppUserImage> AppUserImages { get; set; }
        #endregion
    }
}