using BiologyRecognition.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public interface IUserAccountService
    {
        Task<List<UserAccount>> GetAllAsync();
    }
}
