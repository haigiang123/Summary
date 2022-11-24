using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Model.Models
{
    [Table("AppRolePermissions")]
    public class AppRolePermission
    {
        [Key]
        [Column(Order = 1)]
        public string RoleID { get; set; }

        [Key]
        [Column(Order = 2)]
        public string PermissionId { get; set; }

        [ForeignKey("RoleID")]
        public virtual AppRole AppRole { get; set; }

        [ForeignKey("PermissionId")]
        public virtual AppPermission AppPermission { get; set; }
    }
}
