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

        public async Task<IEnumerable<Category>> GetAll()
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


        public async Task<IList<Category>> GetByCategoryTypeId(int categoryTypeId)
        {
            return await categoryRepository.GetByCategoryTypeId(categoryTypeId);
        }
    }
}
