using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class User
    {
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите Login")]
        [StringLength(30)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Required(ErrorMessage = "Пожалуйста, введите Email")]
        [StringLength(30)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите пароль")]
        [StringLength(30)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        
        [DataType(DataType.Password)]
        [Display(Name = "Повторно Пароль")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }


        [HiddenInput(DisplayValue = false)]
        public bool Confirm { get; set; }


        [HiddenInput(DisplayValue = false)]
        public string Ticket { get; set; }


        
    }
}
