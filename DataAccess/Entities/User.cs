using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("User")]
    public class User : Base.BaseIdEntity
    {
        [Required]
        [Column(TypeName="nvarchar")]
        [MaxLength(2000)]
        [Index("UX_User_Login_GroupId", IsClustered =false, IsUnique =true, Order =1)]
        public string Login { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string Password { get; set; }

        [Required]
        [Index("UX_User_Name_GroupId", IsClustered = false, IsUnique = true, Order = 2)]
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}
