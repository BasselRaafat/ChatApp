using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : BaseEntity
{
    protected readonly AppDbContext _dbContext;

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _dbContext
            .Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(Entity => Entity.Id == id);
    }

    public async Task CreateAsync(T entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbContext.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Remove(entity);
    }
}
