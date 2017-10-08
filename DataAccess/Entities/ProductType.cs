using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("ProductType")]
    public class ProductType : Base.BaseIdEntity
    {
        [Column(TypeName ="nvarchar")]
        [Index("UX_Product_Type_Name", IsUnique = true, Order = 1)]
        [Required]
        [MaxLength(2000)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string Description { get; set; }
    }
}
