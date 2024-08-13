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

    public async Task<List<TDbEntity>> GetAsync()
    {
        return await _context.Set<TDbEntity>().ToListAsync();
    }
}