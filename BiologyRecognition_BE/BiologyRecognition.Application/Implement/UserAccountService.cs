using Azure;
using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using BiologyRecognition.DTOs.UserAccount;
using BiologyRecognition.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Implement
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UserAccountRepository _repository;
        public UserAccountService() => _repository ??= new UserAccountRepository();

        public async Task<PagedResult<UserAccount>> GetAllAsync(int page, int pageSize)
        {
            var query =  _repository.GetAllAsync();
            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(c=> c.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<UserAccount>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
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
