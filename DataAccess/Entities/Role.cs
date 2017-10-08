using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("Role")]
    public class Role : Base.BaseIdEntity
    {
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        [Index("UX_Role_Name", IsClustered = false, IsUnique = true, Order = 1)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string DisplayName { get; set; }
    }
}
