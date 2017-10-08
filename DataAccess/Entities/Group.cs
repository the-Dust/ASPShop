using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("Group")]
    public class Group : Base.BaseIdEntity
    {

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        [Index("UX_Group_Name_RoleId", IsClustered = false, IsUnique = true, Order = 1)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string DisplayName { get; set; }

        [Required]
        [Index("UX_Group_Name_RoleId", IsClustered = false, IsUnique = true, Order = 2)]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
