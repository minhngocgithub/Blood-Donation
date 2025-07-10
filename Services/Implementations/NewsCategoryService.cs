using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blood_Donation_Website.Services.Implementations
{
    public class NewsCategoryService : INewsCategoryService
    {
        public Task<IEnumerable<NewsCategoryDto>> GetAllNewsCategoriesAsync()
            => throw new NotImplementedException();
        public Task<NewsCategoryDto> GetNewsCategoryByIdAsync(int id)
            => throw new NotImplementedException();
        public Task<NewsCategoryDto> CreateNewsCategoryAsync(NewsCategoryDto categoryDto)
            => throw new NotImplementedException();
        public Task<NewsCategoryDto> UpdateNewsCategoryAsync(int id, NewsCategoryDto categoryDto)
            => throw new NotImplementedException();
        public Task<bool> DeleteNewsCategoryAsync(int id)
            => throw new NotImplementedException();
    }
} 