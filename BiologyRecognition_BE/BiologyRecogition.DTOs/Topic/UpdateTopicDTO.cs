using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Topic
{
    public class UpdateTopicDTO
    {
        public int TopicId { get; set; }
        [Required(ErrorMessage = "Bài là bắt buộc.")]
        public int ChapterId { get; set; }

        [Required(ErrorMessage = "Tên nội dung là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên nội dung không được dài quá 100 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mô tả nội dung là bắt buộc.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Người sửa là bắt buộc.")]

        public int? ModifiedBy { get; set; }
    }
}
