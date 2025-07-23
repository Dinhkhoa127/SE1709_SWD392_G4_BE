using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface IUserAccountService
    {
        Task<PagedResult<UserAccount>> GetAllAsync(int page, int pageSize);
        Task<UserAccount> GetUserAccountByIdAsync(int id);
        Task<UserAccount> GetUserAccountByNameOrEmailAsync(string nameOrEmails);
        Task<int> CreateAsync(UserAccount userAccount);
        Task<int> CreateAccountByAdminAsync(UserAccount userAccount);
        Task<UserAccount> GetUserAccountByPhone(string phone);
        Task<int> UpdateAsync(UserAccount userAccount);
    }
}
