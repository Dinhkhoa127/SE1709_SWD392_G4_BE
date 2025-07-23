using Azure;
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
    public interface IChapterService
    {


        Task<int> CreateAsync(Chapter chapter);
        Task<int> UpdateAsync(Chapter chapter);
        Task<List<Chapter>> GetAllAsync();
        Task<PagedResult<Chapter>> GetAllAsync(int page, int pageSize);
        Task<Chapter> GetChapterByNameAsync(string name);
        Task<List<Chapter>> GetListChaptersByContainNameAsync(string name);
        Task<PagedResult<Chapter>> GetListChaptersByContainNameAsync(string name, int page, int pageSize);
        Task<Chapter> GetByIdAsync(int id);
        Task<List<Chapter>> GetListChaptersBySubjectIdAsync(int id);
        Task<PagedResult<Chapter>> GetListChaptersBySubjectIdAsync(int id, int page, int pageSize);
    }
}
