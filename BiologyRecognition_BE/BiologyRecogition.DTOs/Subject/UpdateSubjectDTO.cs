using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.Subject
{
    public class UpdateSubjectDTO
    {
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Tên Subject không được để trống")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Miêu tả không được để trống")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Người update không được để trống")]

        public int ModifiedBy { get; set; }

    }
}
