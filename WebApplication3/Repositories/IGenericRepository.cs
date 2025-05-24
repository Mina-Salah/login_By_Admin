using System.Linq.Expressions;
using WebApplication3.GenaricISpecification;

namespace WebApplication3.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();

        // add spec
        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T?> GetBySpecAsync(ISpecification<T> spec);

    }
}
