using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Article
{
    public class ArticleDetailsDTO
    {
        public int ArticleId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int CreatedBy { get; set; }
        public string CreateName { get; set; }

        public DateTime CreatedDate { get; set; }
        public List<int> ArtifactIds { get; set; }

        public int? ModifiedBy { get; set; }
        public string ModifiedName { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
