using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Model.Models
{
    [Table("AppRoles")]
    public class AppRole : IdentityRole
    {
        public AppRole(string roleName, string description, byte manualId) : base(roleName)
        {
            Description = description;
            ManualId = manualId;
        }
        
        public AppRole() : base()
        {

        }

        public virtual byte? ManualId { get; set; }

        public virtual string Description { get; set; }

        public virtual ICollection<AppRolePermission> AppRolePermissions { get; set; }
    }
}
