using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataAccess.Entities
{
    [Table("Product")]
    public class Product : Base.BaseIdEntity
    {
        [Column(TypeName = "nvarchar")]
        [Index("UX_Product_Name_ProductTypeId", IsUnique = true, Order = 1)]
        [Required(ErrorMessage = "Введите название товара")]
        [MaxLength(2000)]
        [Display(Name = "1. Наименование")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите цену")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите положительное значение для цены")]
        [Display(Name = "5. Цена, руб")]
        public double Cost { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        [Required(ErrorMessage = "Введите описание")]
        [Display(Name = "4. Описание")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        [Required(ErrorMessage = "Введите краткое описание")]
        [Display(Name = "3. Краткое описание")]
        public string ShortDescription { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        [Display(Name = "6. Фото")]
        public string ImageLink { get; set; }

        [Required]
        [Index("UX_Product_Name_ProductTypeId", IsUnique = true, Order = 2)]
        [HiddenInput(DisplayValue = false)]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        [Display(Name = "2. Категория")]
        public virtual ProductType ProductType { get; set; }

    }
}
