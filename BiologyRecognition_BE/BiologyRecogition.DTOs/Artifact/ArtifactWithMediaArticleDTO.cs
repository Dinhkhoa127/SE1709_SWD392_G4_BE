using BiologyRecognition.DTOs.Article;
using BiologyRecognition.DTOs.ArtifactMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Artifact
{
    public class ArtifactWithMediaArticleDTO
    {
        public int ArtifactId { get; set; }
        public string ArtifactName { get; set; }

        public string Description { get; set; }

        public string ScientificName { get; set; }
        public int ArtifactTypeId { get; set; }
        public string ArtifactTypeName { get; set; }

        public int TopicId { get; set; }
        public string TopicName { get; set; }

        public List<ArtifactMediaDTO> MediaList { get; set; } = new();
        public List<ArticleDTO> ArticleList { get; set; } = new();
    }
}
