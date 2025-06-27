using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.UserAccount
{
    public class UpdateAccountStudentPwDTO
    {
        [Required(ErrorMessage = "UserAccountId là bắt buộc.")]
        public int UserAccountId { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]

        public string Password { get; set; }
    }
}
