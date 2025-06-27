using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.ArtifactMedia
{
    public class CreateArtifactMediaDTO
    {

        [Required(ErrorMessage = "Mã Artifact là bắt buộc.")]
        public int ArtifactId { get; set; }

        [Required(ErrorMessage = "Loại media là bắt buộc.")]
        [RegularExpression("^(IMAGE|VIDEO|AUDIO|DOCUMENT)$", ErrorMessage = "Loại media phải là IMAGE, VIDEO, AUDIO hoặc DOCUMENT.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "URL là bắt buộc.")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự.")]
        public string Description { get; set; }
    }
}
