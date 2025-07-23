using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.ArtifactType
{
    public class CreateArtifactTypeDTO
    {
        [Required(ErrorMessage = "Thuộc về nội dung nào là bắt buộc.")]
        public int TopicId { get; set; }
        [Required(ErrorMessage = "Tên loại ArtifactType là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên nội dung không được dài quá 100 ký tự.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Mô tả ArtifactType là bắt buộc.")]
        public string Description { get; set; }
    }
}
