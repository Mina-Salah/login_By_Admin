using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly SchoolContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(SchoolContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        IQueryable<T> query = _dbSet;
        if (include != null)
            query = include(query);

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        IQueryable<T> query = _dbSet;
        if (include != null)
            query = include(query);

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }


    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
