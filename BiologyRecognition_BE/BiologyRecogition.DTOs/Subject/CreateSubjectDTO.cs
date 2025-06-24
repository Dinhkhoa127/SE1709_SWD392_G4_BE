
using System.ComponentModel.DataAnnotations;


namespace BiologyRecognition.DTOs.Subject
{
    public class CreateSubjectDTO
    {
        [Required(ErrorMessage = "Tên Subject không được để trống")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Miêu tả không được để trống")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Người tạo không được để trống")]

        public int CreatedBy { get; set; }

 

        
    }
}
