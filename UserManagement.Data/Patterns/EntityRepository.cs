using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Data.Patterns;

public class EntityRepository<T>(DbContext context) : IEntityRepository<T> where T : class
{
    public async ValueTask<T> AddAsync(T entity)
    {
        return (await context.Set<T>().AddAsync(entity)).Entity;
    }

    public ValueTask<T?> GetAsync(params object[] id)
    {
        return context.Set<T>().FindAsync(id);
    }

    public ValueTask<IEnumerable<T>> AllAsync()
    {
        return new ValueTask<IEnumerable<T>>(context.Set<T>());
    }

    public ValueTask UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        return default;
    }

    public ValueTask DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        return default;
    }
}
