﻿using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Interface
{
    public interface ISubjectService
    {
        Task<Subject> GetSubjectByNameAsync(string name);
        Task<List<Subject>> GetAllAsync();
        Task<PagedResult<Subject>> GetAllAsync(int page, int pageSize);
        Task<Subject> GetSubjectByIdAsync(int id);

        Task<int> CreateAsync(Subject subject);
        Task<int> UpdateAsync(Subject subject);
        Task<bool> DeleteAsync(Subject subject);
        Task<List<Subject>> GetListSubjectByContainNameAsync(string name);
        Task<PagedResult<Subject>> GetListSubjectByContainNameAsync(string name, int page, int pageSize);

    }
}
