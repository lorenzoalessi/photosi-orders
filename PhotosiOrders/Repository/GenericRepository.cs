using Microsoft.EntityFrameworkCore;
using PhotosiOrders.Model;

namespace PhotosiOrders.Repository;

public class GenericRepository<TDbEntity> : IGenericRepository<TDbEntity> where TDbEntity : class
{
    protected readonly Context Context;

    public GenericRepository(Context context)
    {
        Context = context;
    }
    
    public async Task<TDbEntity> AddAsync(TDbEntity dbEntity)
    {
        await Context.Set<TDbEntity>().AddAsync(dbEntity);
        await Context.SaveChangesAsync();

        return dbEntity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await Context.Set<TDbEntity>().FindAsync(id);
        if (entity == null)
            return false;
            
        Context.Set<TDbEntity>().Remove(entity);
        await Context.SaveChangesAsync();

        return true;
    }

    public async Task SaveAsync() => await Context.SaveChangesAsync();
}