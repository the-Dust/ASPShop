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
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите цену")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите положительное значение для цены")]
        [Display(Name = "Цена, руб")]
        public double Cost { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        [Display(Name = "Краткое описание")]
        public string ShortDescription { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(2000)]
        [Display(Name = "Фото")]
        public string ImageLink { get; set; }

        [Required]
        [Index("UX_Product_Name_ProductTypeId", IsUnique = true, Order = 2)]
        [HiddenInput(DisplayValue = false)]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        [Display(Name = "Категория")]
        public virtual ProductType ProductType { get; set; }

    }
}
