using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Recognition
{
    public class RecognitionDTO
    {
        public int RecognitionId { get; set; }

        public int? ArtifactId { get; set; }

        public int UserId { get; set; }

        public string ImageUrl { get; set; }

        public DateTime RecognizedAt { get; set; }

        public double? ConfidenceScore { get; set; }

        public string AiResult { get; set; }

        public string Status { get; set; }
        public string ArtifactName { get; set; }


    }
}
