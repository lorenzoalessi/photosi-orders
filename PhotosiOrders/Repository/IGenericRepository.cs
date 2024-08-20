namespace PhotosiOrders.Repository;

public interface IGenericRepository<TDbEntity>
{
    Task<TDbEntity> AddAsync(TDbEntity dbEntity);
    
    Task<bool> DeleteAsync(int id);

    Task SaveAsync();
}