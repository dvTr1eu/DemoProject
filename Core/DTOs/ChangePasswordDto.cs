using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Current Password is required.")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(8, ErrorMessage = "Bạn cần đặt mật khẩu ít nhất 8 ký tự")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [MinLength(8, ErrorMessage = "Bạn cần đặt mật khẩu ít nhất 8 ký tự")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không giống nhau")]
        public string ConfirmPassword { get; set; }
    }
}
