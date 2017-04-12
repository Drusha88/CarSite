using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Car
    {
        [HiddenInput(DisplayValue = false)]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название автомобиля")]
        [StringLength(100)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите марку автомобиля")]
        [StringLength(100)]
        [Display(Name = "Марка")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите описание автомобиля")]
        [StringLength(600)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите категорию автомобиля")]
        [StringLength(50)]
        [Display(Name = "Категория")]
        public string Category { get; set; }

        [HiddenInput(DisplayValue = false)]
         public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }
    }
}
