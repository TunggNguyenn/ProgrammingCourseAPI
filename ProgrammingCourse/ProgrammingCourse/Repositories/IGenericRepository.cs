using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task AddRange(List<T> entities);
        Task Remove(T entity);
        Task RemoveRange(List<T> entities);
        Task Update(T entity);
        Task UpdateRange(List<T> entities);
    }
}
