using System.ComponentModel.DataAnnotations;

namespace BiologyRecognition.DTOs.UserAccount
{
    public class UpdateAccountStudentDTO
    {
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
        [Required(ErrorMessage = "Ngưởi sửa không được để trống")]

        public string ModifiedBy { get; set; }

    }
}
