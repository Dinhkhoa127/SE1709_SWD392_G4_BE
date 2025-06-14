
using System.ComponentModel.DataAnnotations;

namespace BiologyRecognition.DTOs.UserAccount
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Mã nhân viên không được để trống")]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "Role không được để trống")]
        public int? RoleId { get; set; }

        [Required(ErrorMessage = "RequestCode không được để trống")]
        public string RequestCode { get; set; }

        [Required(ErrorMessage = "ApplicationCode không được để trống")]
        public string ApplicationCode { get; set; }

        [Required(ErrorMessage = "Người tạo không được để trống")]
        public string CreatedBy { get; set; }
    }

}
