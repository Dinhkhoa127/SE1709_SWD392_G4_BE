using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Topic
{
    public class TopicDTO
    {
        public int TopicId { get; set; }
        public int ChapterId { get; set; }
        public string Name { get; set; }
        public string ChapterName { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }
        public string ModifiedName { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
