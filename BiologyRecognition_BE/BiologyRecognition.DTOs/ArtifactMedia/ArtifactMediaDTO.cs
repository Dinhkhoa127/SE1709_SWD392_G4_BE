using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.ArtifactMedia
{
    public class ArtifactMediaDTO
    {
        public int ArtifactMediaId { get; set; }

        public int ArtifactId { get; set; }
        public string ArtifactName { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

    }
}
