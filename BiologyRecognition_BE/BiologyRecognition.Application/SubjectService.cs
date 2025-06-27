using BiologyRecognition.Domain.Entities;
using BiologyRecognition.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application
{
    public class SubjectService : ISubjectService
    {
        private readonly SubjectRepository _repository;
        public SubjectService() => _repository ??= new SubjectRepository();

        public Task<Subject> GetSubjectByNameAsync(string name)
        {
            return _repository.GetSubjectByNameAsync(name);
        }

        public Task<List<Subject>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Subject> GetSubjectByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<int> CreateAsync(Subject subject)
        {
            return _repository.CreateAsync(subject);
        }

        public Task<int> UpdateAsync(Subject subject)
        {
            return _repository.UpdateAsync(subject);
        }

        public Task<List<Subject>> GetListSubjectByContainNameAsync(string name)
        {
            return _repository.GetListSubjectByContainNameAsync(name);
        }
        public Task<bool> DeleteAsync(Subject subject)
        {
            return _repository.RemoveAsync(subject);
        }
    }
}
