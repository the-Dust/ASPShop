using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    [Table("Product")]
    public class Product : Base.BaseIdEntity
    {
        [Column(TypeName = "nvarchar")]
        [Index("UX_Product_Name_ProductTypeId", IsUnique = true, Order = 1)]
        [Required]
        [MaxLength(2000)]
        public string Name { get; set; }

        [Required]
        public double Cost { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string ShortDescription { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        public string ImageLink { get; set; }

        [Required]
        [Index("UX_Product_Name_ProductTypeId", IsUnique = true, Order = 2)]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public virtual ProductType ProductType { get; set; }

    }
}
