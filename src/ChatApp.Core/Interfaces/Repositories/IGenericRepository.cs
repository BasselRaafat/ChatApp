using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces.Repositories;

public interface IGenericRepository<T>
    where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);

    Task SaveChangesAsync();
}
