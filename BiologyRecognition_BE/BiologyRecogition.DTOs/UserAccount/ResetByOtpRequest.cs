using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.UserAccount
{
    public class ResetByOtpRequest
    {
        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mã OTP không được để trống.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Mã OTP phải gồm 6 chữ số.")]
        public string OtpCode { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống.")]
        public string NewPassword { get; set; }
    }
}
