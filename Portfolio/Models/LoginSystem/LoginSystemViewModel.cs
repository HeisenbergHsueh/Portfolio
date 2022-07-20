using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Models.LoginSystem
{
    /// <summary>
    /// 給user進行登入的view model，因此僅有UserName跟Password兩個屬性
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [Remote]
        public string UserName { get; set; }

        [Required(ErrorMessage = "請輸入密碼"), MaxLength(50)]
        //[DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "請輸入帳號"), MaxLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "請輸入密碼"), MaxLength(50)]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "請再一次輸入密碼"), MaxLength(50)]
        [DataType(DataType.Password), Compare(nameof(UserPassword), ErrorMessage = "密碼不相符")]
        public string ComfirmUserPassword { get; set; }

        [Required(ErrorMessage = "請輸入Email"), MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        //[DataType(DataType.EmailAddress, ErrorMessage = "Email格式不正確")]
        public string UserEmail { get; set; }
    }
}
