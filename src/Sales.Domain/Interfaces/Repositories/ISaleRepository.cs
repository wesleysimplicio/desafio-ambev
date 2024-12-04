using Sales.Domain.Entities;

namespace Sales.Domain.Interfaces.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale?> GetByIdAsync(int id);
        Task<List<Sale>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<Sale> AddAsync(Sale sale);
        Task<Sale> UpdateAsync(Sale sale);
    }
}
