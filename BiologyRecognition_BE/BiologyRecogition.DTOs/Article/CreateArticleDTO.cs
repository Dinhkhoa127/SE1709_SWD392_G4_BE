using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Article
{
    public class CreateArticleDTO
    {


        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Bài viết phải gắn với ít nhất một hiện vật.")]
        [MinLength(1, ErrorMessage = "Phải chọn ít nhất một hiện vật.")]
        public List<int> ArtifactIds { get; set; }

        [Required(ErrorMessage = "Người tạo không được để trống")]
        public int CreatedBy { get; set; }
    


    }
}
