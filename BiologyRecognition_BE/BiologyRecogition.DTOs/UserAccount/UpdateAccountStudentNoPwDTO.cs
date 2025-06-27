using System.ComponentModel.DataAnnotations;

namespace BiologyRecognition.DTOs.UserAccount
{
    public class UpdateAccountStudentNoPwDTO
    {
        [Required(ErrorMessage = "UserAccountId là bắt buộc.")]
        public int UserAccountId { get; set; }
        
        [Required(ErrorMessage = "Họ tên không được để trống")]

        public string FullName { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0")]

        public string Phone { get; set; }
    

    }
}
