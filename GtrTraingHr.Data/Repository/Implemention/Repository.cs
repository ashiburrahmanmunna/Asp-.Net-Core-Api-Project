using GtrTraingHr.Data.Repository.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GtrTraingHr.Data.Repository.Implemention
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly GtrDbContext db;

        public Repository(GtrDbContext db)
        {
            this.db = db;
        }
        public async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            return await db.Set<T>().CountAsync(predicate);
        }
        public async Task<SelectList> DropDownData(Expression<Func<T, bool>>? predicate,string value, string text)
        {
            if (predicate is null)
            {
                return  new SelectList(await db.Set<T>().ToListAsync(), value, text);
            }
            return new SelectList(await db.Set<T>().Where(predicate).ToListAsync(), value, text);

        }
        public async Task Create(T model)
        {
            await db.Set<T>().AddAsync(model);
            await SaveChanges();
        }

        public async Task Delete(string Id)
        {
            var result = await GetById(Id);
            if (result is not null)
            {
                db.Set<T>().Remove(result);
                await SaveChanges();
            }
        }

        public async Task<bool> Exist(Expression<Func<T, bool>> predicate)
        {
            return await db.Set<T>().AnyAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await db.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(string id)
        {
            return await db.Set<T>().FindAsync(id);
        }

        public async Task<T> Single(Expression<Func<T, bool>> predicate)
        {
            return await db.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public async Task<T> Update(T model, string Id)
        {
            if (model == null)
            {
                return null;
            }
            T oldData = await db.Set<T>().FindAsync(Id);
            if (oldData != null)
            {
                db.Entry(oldData).CurrentValues.SetValues(model);
                await SaveChanges();
            }
            return model;
        }

        public  IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return  db.Set<T>().Where(predicate);
        }
        protected async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }
    }
}
