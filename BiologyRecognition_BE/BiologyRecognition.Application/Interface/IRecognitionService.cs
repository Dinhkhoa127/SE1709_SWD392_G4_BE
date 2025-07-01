using BiologyRecognition.Domain.Entities;
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


        Task<Recognition> GetByIdAsync(int id);
        Task<List<Recognition>> GetRecognitionUserByIdAsync(int userId);

    }
}
