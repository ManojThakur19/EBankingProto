using Microsoft.EntityFrameworkCore;
using MigrationHelper.Data;
using MigrationHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DbHelper
{
    public class Repository<T> : IRepository<T> where T : MainModel
    {
        private readonly ApplicationDbContext _dataContext;

        public Repository(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public T FindById(int id)
        {
            return _dataContext.Set<T>().AsNoTracking().FirstOrDefault(x => x.ID == id);
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _dataContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
        }

        public ICollection<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dataContext.Set<T>().AsNoTracking().Where(predicate).ToList();
        }

        public async Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dataContext.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public ICollection<T> FindAll()
        {
            return _dataContext.Set<T>().AsNoTracking().ToList();
        }

        public async Task<ICollection<T>> FindAllAsync()
        {
            return await _dataContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> FindLastAsync()
        {
            return await _dataContext.Set<T>().AsNoTracking().LastAsync();
        }

        public T Add(T item, bool saveChanges = true)
        {
            _dataContext.Set<T>().Add(item);
            if (saveChanges) _dataContext.SaveChanges();
            return item;
        }

        public async Task<T> AddAsync(T item, bool saveChanges = true)
        {
            _dataContext.Set<T>().Add(item);
            if (saveChanges) await _dataContext.SaveChangesAsync();
            return item;
        }

        public T Update(T item, int id, bool saveChanges = true)
        {
            if (item == null) return null;

            var existing = _dataContext.Set<T>().FirstOrDefault(x => x.ID == id);
            if (existing == null) return null;

            _dataContext.Entry(existing).CurrentValues.SetValues(item);
            _dataContext.Entry(existing).State = EntityState.Modified;

            if (saveChanges) _dataContext.SaveChanges();

            return existing;
        }

        public async Task<T> UpdateAsync(T item, int id, bool saveChanges = true)
        {
            if (item == null) return null;

            var existing = await _dataContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
            if (existing == null) return null;

            _dataContext.Entry(existing).CurrentValues.SetValues(item);
            _dataContext.Entry(existing).State = EntityState.Modified;

            if (saveChanges) await _dataContext.SaveChangesAsync();

            return existing;
        }

        public void Remove(T item, bool saveChanges = true)
        {
            _dataContext.Set<T>().Remove(item);
            if (saveChanges) _dataContext.SaveChanges();
        }

        public async Task RemoveAsync(T item, bool saveChanges = true)
        {
            _dataContext.Set<T>().Remove(item);
            if (saveChanges) await _dataContext.SaveChangesAsync();
        }

        public void RemoveById(int id, bool saveChanges = true)
        {
            var existing = _dataContext.Set<T>().AsNoTracking().FirstOrDefault(x => x.ID == id);
            if (existing == null) return;

            _dataContext.Set<T>().Remove(existing);
            if (saveChanges) _dataContext.SaveChanges();
        }

        public async Task RemoveByIdAsync(int id, bool saveChanges = true)
        {
            var existing = await _dataContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
            if (existing == null) return;

            _dataContext.Set<T>().Remove(existing);
            if (saveChanges) await _dataContext.SaveChangesAsync();
        }

        public ICollection<T> Page(int pageSize, int page)
        {
            return _dataContext.Set<T>().Skip(page * pageSize).Take(pageSize).ToList();
        }

        public async Task<ICollection<T>> PageAsync(int pageSize, int page)
        {
            return await _dataContext.Set<T>().Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        public int SaveChanges()
        {
            try
            {
                return _dataContext.SaveChanges();
            }
            catch (Exception)
            {
                //TODO: save error log information

                return 0;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _dataContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO: save error log information

                return 0;
            }
        }

    }
}
