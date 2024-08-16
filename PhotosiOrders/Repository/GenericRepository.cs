using Microsoft.EntityFrameworkCore;
using PhotosiOrders.Model;

namespace PhotosiOrders.Repository;

public class GenericRepository<TDbEntity> : IGenericRepository<TDbEntity> where TDbEntity : class
{
    protected readonly Context _context;

    public GenericRepository(Context context)
    {
        _context = context;
    }
    
    public async Task<TDbEntity> AddAsync(TDbEntity dbEntity)
    {
        await _context.Set<TDbEntity>().AddAsync(dbEntity);
        await _context.SaveChangesAsync();

        return dbEntity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Set<TDbEntity>().FindAsync(id);
        if (entity == null)
            return false;
            
        _context.Set<TDbEntity>().Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}