using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Artifact
{
    public class ArtifactDTO
    {
        public int ArtifactId { get; set; }
        public string ArtifactName { get; set; }

        public string Description { get; set; }

        public string ScientificName { get; set; }
        public int ArtifactTypeId { get; set; }
        public string ArtifactTypeName { get; set; }
   
        public int TopicId { get; set; }
        public string TopicName { get; set; }
    }
}
