using BiologyRecognition.DTOs.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface IAuthenticationService
    {
        Task<AccountResponseDTO> Login(LoginDTO loginDTO);
        Task<RegisterDTO> Register(RegisterDTO registerDTO);
        Task<string> GenerateToken(AccountResponseDTO accountResponseDTO);
        Task<AccountResponseDTO> LoginWithGoolgle(string email, string name);

        Task<AccountResponseDTO> GetCurrentUser();
    }
}
