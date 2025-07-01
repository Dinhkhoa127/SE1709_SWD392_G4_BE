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

       public string ArtifactName { get; set; }


    }
}
