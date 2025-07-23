using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.ArtifactType
{
    public class ArtifactTypeDTO
    {
        public int ArtifactTypeId { get; set; }

        public int TopicId { get; set; }
        public string TopicName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
