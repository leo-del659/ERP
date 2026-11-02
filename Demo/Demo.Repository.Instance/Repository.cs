using System.Linq.Expressions;
using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.EFCore;
using Demo.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repository.Instance;

[Inject_Attribute]
public class Repository<T> : I_Repository<T> where T : class
{
    protected readonly ErpDbContext _db;

    public Repository(ErpDbContext db)
    {
        _db = db;
    }

    public IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null)
    {
        var q = _db.Set<T>().AsQueryable();
        return predicate == null ? q : q.Where(predicate);
    }

    public Task<T?> FindAsync(params object[] keys) => _db.Set<T>().FindAsync(keys).AsTask();

    public async Task AddAsync(T entity) => await _db.Set<T>().AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities) => await _db.Set<T>().AddRangeAsync(entities);

    public void Update(T entity) => _db.Set<T>().Update(entity);

    public void Remove(T entity) => _db.Set<T>().Remove(entity);

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}
