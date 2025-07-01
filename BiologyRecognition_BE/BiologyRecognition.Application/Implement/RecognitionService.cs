using BiologyRecognition.Application.Interface;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Implement
{
    public class RecognitionService : IRecognitionService
    {
        private readonly RecognitionRepository _repository;
        public RecognitionService() => _repository ??= new RecognitionRepository();
        public Task<List<Recognition>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Recognition> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<Recognition>> GetRecognitionUserByIdAsync(int userId)
        {
            return _repository.GetRecognitionUserByIdAsync(userId);
        }
    }
}
