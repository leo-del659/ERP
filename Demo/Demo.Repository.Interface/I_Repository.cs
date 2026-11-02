using System.Linq.Expressions;

namespace Demo.Repository.Interface;

[Demo.Infrastructure.Attributes.Provider_Attribute]
public interface I_Repository<T> where T : class
{
    IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null);
    Task<T?> FindAsync(params object[] keys);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    Task<int> SaveChangesAsync();
}
