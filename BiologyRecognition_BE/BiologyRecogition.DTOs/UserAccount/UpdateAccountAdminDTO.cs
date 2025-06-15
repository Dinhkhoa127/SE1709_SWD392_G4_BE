using System.ComponentModel.DataAnnotations;

namespace BiologyRecognition.DTOs.UserAccount
{
    public class UpdateAccountAdminDTO
    {
        [Required]
        public int UserAccountId { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]

        public string Password { get; set; }
        [Required(ErrorMessage = "Họ tên không được để trống")]

        public string FullName { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]

        public string Phone { get; set; }


        [Required(ErrorMessage = "Mã nhân viên không được để trống")]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "Role không được để trống")]
        public int? RoleId { get; set; }

        [Required(ErrorMessage = "RequestCode không được để trống")]
        public string RequestCode { get; set; }

        [Required(ErrorMessage = "ApplicationCode không được để trống")]
        public string ApplicationCode { get; set; }

        [Required(ErrorMessage = "Ngưởi sửa không được để trống")]

        public string ModifiedBy { get; set; }
        [Required(ErrorMessage = "Trạng thái không được để trống")]

        public bool? IsActive { get; set; }
    }
}
