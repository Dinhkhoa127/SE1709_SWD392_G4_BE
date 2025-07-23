using BiologyRecognition.Domain.Entities;
using BiologyRecognition.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UserAccountRepository _repository;
        public UserAccountService() => _repository ??= new UserAccountRepository();

        public async Task<UserAccount> GetAccountByIdAsync(int id)
        {
           return await _repository.GetByIdAsync(id);
        }

        public async Task<List<UserAccount>> GetAllAsync()
        {
           return await _repository.GetAllAsync();
        }

    }
}
