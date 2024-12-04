using Sales.Domain.Models;

namespace Sales.Domain.Interfaces.Services
{
    public interface ISaleService
    {
        Task<ServicesResponse<SaleModel>> GetByIdAsync(int idSale);
        Task<ServicesResponse<List<SaleModel>>> GetAllAsync();

        Task<ServicesResponse<SaleModel>> UpdateAsync(int saleId, SaleModel saleAUpdate);

        Task<ServicesResponse<SaleModel>> DeleteAsync(int idSale);
        Task<ServicesResponse<SaleModel>> AddAsync(SaleModel saleReq);
    }
}
