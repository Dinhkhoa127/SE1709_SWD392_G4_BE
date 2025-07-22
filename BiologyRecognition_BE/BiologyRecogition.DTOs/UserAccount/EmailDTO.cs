using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.UserAccount
{
    public class EmailDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
