using ProgrammingCourse.Models;
using ProgrammingCourse.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Services
{
    public class CategoryService
    {
        protected readonly CategoryRepository categoryRepository;

        public CategoryService(CategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task Add(Category category)
        {
            await categoryRepository.Add(category);
        }

        public async Task<Category> GetById(int id)
        {
            return await categoryRepository.GetById(id);
        }

        public async Task<Category> GetWithAllInfoById(int id)
        {
            return await categoryRepository.GetWithAllInfoById(id);
        }

        public async Task<Category> GetWithAllInfoByName(string name)
        {
            return await categoryRepository.GetWithAllInfoByName(name);
        }

        public async Task<List<Category>> GetAll()
        {
            return await categoryRepository.GetAll();
        }

        public async Task Remove(Category category)
        {
            await categoryRepository.Remove(category);
        }


        public async Task Update(Category category)
        {
            await categoryRepository.Update(category);
        }

        public async Task<dynamic> GetMostRegisteredCategories()
        {
            return await categoryRepository.GetMostRegisteredCategories();
        }
    }
}
