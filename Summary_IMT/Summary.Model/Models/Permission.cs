using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Summary.Model.Models
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int ID { get; set; }

        [StringLength(128)]
        public string RoleId { get; set; }

        [StringLength(50)]
        [Column(TypeName ="varchar")]
        public string FunctionId { get; set; }

        public bool CanCreate { set; get; } 

        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }

        public bool CanDelete { set; get; }

        [ForeignKey("FunctionId")]
        public Function Function { get; set; }
    }
}