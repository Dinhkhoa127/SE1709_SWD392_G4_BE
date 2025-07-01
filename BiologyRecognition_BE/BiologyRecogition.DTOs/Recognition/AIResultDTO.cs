using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Recognition
{
    public class AIResultDTO
    {
        [JsonPropertyName("label")]
        public string ArtifactName { get; set; }

        [JsonPropertyName("similarity")]
        public double Confidence { get; set; }
    }
}
