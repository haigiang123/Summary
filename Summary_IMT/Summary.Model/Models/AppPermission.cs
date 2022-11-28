using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Model.Models
{
    [Table("AppPermissions")]
    public class AppPermission
    {
        [Key]
        public string Id { get; set; }

        public byte? ManualId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<AppRolePermission> AppRolePermissions { get; set; }
    }
}
