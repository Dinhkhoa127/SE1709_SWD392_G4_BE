

namespace BiologyRecognition.DTOs.Subject
{
    public class SubjectDTO
    {
        public int SubjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
