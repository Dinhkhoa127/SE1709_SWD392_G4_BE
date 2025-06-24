using BiologyRecognition.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public interface ISubjectService
    {
        Task<Subject> GetSubjectByNameAsync(string name);
        Task<List<Subject>> GetAllAsync();
        Task<Subject> GetSubjectByIdAsync(int id);
     
        Task<int> CreateAsync(Subject subject);
        Task<int> UpdateAsync(Subject subject);
        Task<List<Subject>> GetListSubjectByContainNameAsync(string name);
       
    }
}
