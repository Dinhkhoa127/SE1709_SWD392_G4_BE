using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Artifact
{
    public class UpdateArtifactDTO
    {
        [Required(ErrorMessage = "Loại Artifact là bắt buộc.")]
        public int ArtifactId { get; set; }
        [Required(ErrorMessage = "Loại ArtifactType là bắt buộc.")]
        public int ArtifactTypeId { get; set; }

        [Required(ErrorMessage = "Mã Artifact là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Mã Artifact không được vượt quá 50 ký tự.")]
        [RegularExpression(@"^ART.*$", ErrorMessage = "Mã Artifact phải bắt đầu bằng 'ART'.")]
        public string ArtifactCode { get; set; }

        [Required(ErrorMessage = "Tên Artifact là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên Artifact không được dài quá 100 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mô tả Artifact là bắt buộc.")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Tên khoa học là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên khoa học không được dài quá 100 ký tự.")]
        public string? ScientificName { get; set; }


        [Required(ErrorMessage = "Người sửa là bắt buộc.")]
        public int? ModifiedBy { get; set; }
    }
}
