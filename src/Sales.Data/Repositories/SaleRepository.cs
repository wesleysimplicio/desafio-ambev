using Microsoft.EntityFrameworkCore;
using Sales.Data.Contexts;
using Sales.Domain.Entities;
using Sales.Domain.Interfaces.Repositories;

namespace Sales.Data.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SalesContext _context;

        public SaleRepository(SalesContext context)
        {
            _context = context;
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _context.Sales.AsNoTracking()
                .Include(v => v.Client)
                .Include(v => v.Branch)
                .Include(v => v.Itens)
                    .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<Sale?> GetByIdAsync(int id)
        {
            return await _context.Sales.AsNoTracking()
                .Include(v => v.Client)
                .Include(v => v.Branch)
                .Include(v => v.Itens)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(v => v.SaleId == id);
        }

        public async Task<Sale> AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<Sale> UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task DeleteAsync(int id)
        {
            var sale = await GetByIdAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }
    }
}
