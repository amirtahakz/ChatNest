﻿using System.ComponentModel.DataAnnotations;

namespace ChatNest.Core.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "نام کاربری اجباری است")]
        public string UserName { get; set; }
        [Required(ErrorMessage = " کلمه عبور اجباری است")]
        public string Password { get; set; }
    }
}