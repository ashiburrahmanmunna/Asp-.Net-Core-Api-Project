using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace GtrTraingHr.Data.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        Task<T> Single(Expression<Func<T, bool>> predicate);
        Task<T> GetById(string id);
        Task Create(T model);
        Task<T> Update(T model, string Id);
        Task Delete(string Id);
        Task<int> Count(Expression<Func<T, bool>> predicate);
        Task<bool> Exist(Expression<Func<T, bool>> predicate);
        Task<SelectList> DropDownData(Expression<Func<T, bool>>? predicate, string value, string text);
    }
}
