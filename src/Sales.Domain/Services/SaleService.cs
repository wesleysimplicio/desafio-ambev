using AutoMapper;
using Microsoft.Extensions.Logging;
using Sales.Domain.Entities;
using Sales.Domain.Interfaces.Repositories;
using Sales.Domain.Interfaces.Services;
using Sales.Domain.Models;

namespace Sales.Domain.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public SaleService(ISaleRepository saleRepository, ILogger<SaleService> logger, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServicesResponse<List<SaleModel>>> GetAllAsync()
        {
            var sales = await _saleRepository.GetAllAsync();

            if (sales == null || !sales.Any())
            {
                return new ServicesResponse<List<SaleModel>>(false, null, "Nenhuma sale encontrada.");
            }

            var modelSales = _mapper.Map<List<SaleModel>>(sales);

            return new ServicesResponse<List<SaleModel>>(true, modelSales, "Sales obtidas com success.");
        }

        public async Task<ServicesResponse<SaleModel>> GetByIdAsync(int idSale)
        {
            var sale = await _saleRepository.GetByIdAsync(idSale);

            if (sale == null)
            {
                return new ServicesResponse<SaleModel>(false, null, $"Sale com ID {idSale} não encontrada.");
            }

            var modelSale = _mapper.Map<SaleModel>(sale);

            return new ServicesResponse<SaleModel>(true, modelSale, "Sale obtida com success.");
        }

        public async Task<ServicesResponse<SaleModel>> AddAsync(SaleModel saleReq)
        {
            saleReq.DateSale = DateTime.UtcNow;

            saleReq.ValueTotal = saleReq.Itens.Sum(item => item.Quantity * (item.PriceUnit - item.Discount));

            var sale = _mapper.Map<Sale>(saleReq);
            var novaSale = _mapper.Map<SaleModel>(await _saleRepository.AddAsync(sale));

            return new ServicesResponse<SaleModel>(true, novaSale, "Sale adicionada com success.");
        }

        public async Task<ServicesResponse<SaleModel>> UpdateAsync(int saleId, SaleModel saleAtualizada)
        {
            var saleExistente = await _saleRepository.GetByIdAsync(saleId);

            if (saleExistente == null)
            {
                return new ServicesResponse<SaleModel>(false, null, $"Sale com ID {saleId} não encontrada.");
            }

            saleExistente.ClientId = saleAtualizada.IdClient;
            saleExistente.BranchId = saleAtualizada.IdBranch;
            saleExistente.StatusSale = saleAtualizada.StatusSale;

            saleExistente.Itens = saleAtualizada.Itens.ConvertAll(i => new Entities.ItemSale
            {
                ItemSaleId = i.ItemSaleId,
                SaleId = saleExistente.SaleId,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                PriceUnit = i.PriceUnit,
                Discount = i.Discount,
                Product = null!
            });

            saleExistente.ValueTotal = saleExistente.Itens.Sum(item => item.Quantity * (item.PriceUnit - item.Discount));

            var saleAtualizadaFinal = _mapper.Map<SaleModel>(await _saleRepository.UpdateAsync(saleExistente));

            return new ServicesResponse<SaleModel>(true, saleAtualizadaFinal, "Sale atualizada com success.");
        }

        public async Task<ServicesResponse<SaleModel>> DeleteAsync(int idSale)
        {
            var saleExistente = await _saleRepository.GetByIdAsync(idSale);
            bool success = true;
            string msg = string.Empty;
            if (saleExistente == null)
            {
                success = false;
                msg = $"Venda com ID {idSale} não encontrada.";
            }
            else
            {
                await _saleRepository.DeleteAsync(idSale);
                msg = "VEnda removida com success.";
            }

            return new ServicesResponse<SaleModel>(success, null, msg);
        }
    }
}
