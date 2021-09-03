using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace StoreBackend.Common.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetOneAsync(int id);
        Task CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(IEnumerable<T> entities);

        Task<IIncludableQueryable<T, object>> IncludeAsync(Expression<Func<T, object>> navigationPropertyPath);
        Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
    }
}