using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using StoreBackend.Common.Interfaces;
using StoreBackend.DbContexts;

namespace StoreBackend.Common
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;
        protected DbSet<T> _entity;

        public BaseRepository(StoreDbContext context, DbSet<T> entity)
        {
            _context = context;
            _entity = entity;
        }
        public async Task<IEnumerable<T>> GetAllAsync() => await _entity.ToListAsync();
        public async Task<T> GetOneAsync(int id) => await _entity.FindAsync(id);
        public async Task CreateAsync(T entity)
        {
            _entity.Add(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            bool entityFound = false;

            try
            {
                var entryFromDb = _entity.First(e => e.Id == entity.Id);
                entity = ModifyNewEntityBeforeUpdate(entryFromDb, entity);
                _context.Entry(entryFromDb).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                entityFound = true;
            }
            catch (Exception exception)
            {
                if (exception is DbUpdateConcurrencyException || exception is InvalidOperationException)
                {
                    if (!EntityExists(entity.Id))
                    {
                        entityFound = false;
                    }
                }
                else
                {
                    throw;
                }
            }

            return entityFound;
        }


        public async Task DeleteAsync(T entity)
        {
            _entity.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<T> entities)
        {
            _entity.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<IIncludableQueryable<T, object>> IncludeAsync(Expression<Func<T, object>> navigationPropertyPath)
        => await Task.Run(() => _entity.Include(navigationPropertyPath));

        public Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> predicate) => Task.Run(() => _entity.Where(predicate));

        private bool EntityExists(int id)
        {
            return _entity.Any(e => e.Id == id);
        }
        protected virtual T ModifyNewEntityBeforeUpdate(T entityFromDb, T newEntity) => newEntity;
    }
}