using BiologyRecognition.Domain.DBContext;
using BiologyRecognition.Domain.Entities;
using BloodDonation.Repositories.NhanNB.Basic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Infrastructure
{
    public class UserAccountRepository : GenericRepository<UserAccount>
    {
        public UserAccountRepository() { }
        public UserAccountRepository(SE1709_SWD392_G4_BiologyRecognitionSystemContext context) => _context = context;

        public async Task<UserAccount> GetUserAccountByNameOrEmailAsync(string nameOrEmails)
        {
            return await _context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == nameOrEmails || u.Email == nameOrEmails);
        }
        public async Task<UserAccount> GetUserAccountByPhone(string phone)
        {
            return await _context.UserAccounts.FirstOrDefaultAsync(u => u.Phone == phone.Trim());
        }
    }
}
