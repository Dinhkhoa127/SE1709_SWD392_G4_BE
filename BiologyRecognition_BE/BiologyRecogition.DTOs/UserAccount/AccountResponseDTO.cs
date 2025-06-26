using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.DTOs.UserAccount
{
    public class AccountResponseDTO
    {
        public int UserAccountId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EmployeeCode { get; set; }
        public int? RoleId { get; set; }
        public string RequestCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ApplicationCode { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
        public string AccessToken { get; set; }

    }
}
