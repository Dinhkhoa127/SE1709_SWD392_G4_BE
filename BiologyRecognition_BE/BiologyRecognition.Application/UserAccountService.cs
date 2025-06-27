using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.UserAccount;
using BiologyRecognition.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UserAccountRepository _repository;
        public UserAccountService() => _repository ??= new UserAccountRepository();

        public async Task<List<UserAccount>> GetAllAsync()
        {
           return await _repository.GetAllAsync();
        }
        public async Task<UserAccount> GetUserAccountByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<UserAccount> GetUserAccountByNameOrEmailAsync(string nameOrEmails)
        {
            return await _repository.GetUserAccountByNameOrEmailAsync(nameOrEmails);
        }
        public async Task<int> CreateAsync(UserAccount userAccount)
        {
            return await _repository.CreateAsync(userAccount);
        }
        public async Task<int> CreateAccountByAdminAsync(UserAccount userAccount)
        {
            userAccount.Password = BCrypt.Net.BCrypt.HashPassword(userAccount.Password);
            return await _repository.CreateAsync(userAccount);
        }
        public async Task<UserAccount> GetUserAccountByPhone(string phone)
        {
            return await _repository.GetUserAccountByPhone(phone);
        }
        public async Task<int> UpdateAsync(UserAccount userAccount)
        {
           
            return await _repository.UpdateAsync(userAccount);
        }
    }
}
