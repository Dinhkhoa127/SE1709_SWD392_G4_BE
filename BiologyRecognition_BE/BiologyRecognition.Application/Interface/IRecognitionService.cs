using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using BiologyRecognition.DTOs.Recognition;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface IRecognitionService
    {
        Task<List<Recognition>> GetAllAsync();

        Task<PagedResult<Recognition>> GetAllAsync(int page, int pageSize);
        Task<Recognition> GetByIdAsync(int id);
        Task<List<Recognition>> GetRecognitionUserByIdAsync(int userId);
        Task<PagedResult<Recognition>> GetRecognitionUserByIdAsync(int userId, int page, int pageSize);
        Task<int> CreatAsync (Recognition recognition);
        Task<int> DeleteExpiredRecognitionsAsync(CancellationToken cancellationToken = default);
        Task<Recognition> CreateFailedRecognition(ImageDTO imageDTO, double confidence);
        Task<Recognition> CreateSuccessRecognition(ImageDTO imageDTO, Artifact firstArtifact, double confidence);
    }
}
