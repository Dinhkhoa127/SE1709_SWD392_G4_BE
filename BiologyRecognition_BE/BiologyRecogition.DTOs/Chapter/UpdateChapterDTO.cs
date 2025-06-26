using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Chapter
{
    public class UpdateChapterDTO
    {
       
        public int ChapterId { get; set; }
        [Required(ErrorMessage = "Chương là bắt buộc.")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Tên bài là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên bài không được dài quá 100 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mô tả bài là bắt buộc.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Người sửa là bắt buộc.")]

        public int? ModifiedBy { get; set; }

    }
}
