using System.ComponentModel.DataAnnotations;

namespace BiologyRecognition.DTOs.UserAccount
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string UserNameOrEmail { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]

        public string Password { get; set; }
    }
}
