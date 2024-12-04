using AutoMapper;
using Sales.Domain.Entities;
using Sales.Domain.Models;

namespace Sales.Domain.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<Sale, SaleModel>()
                .ForMember(dest => dest.IdSale, opt => opt.MapFrom(src => src.SaleId))
                .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client.NameClient))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch.NameBranch));

            CreateMap<ItemSale, ItemSaleModel>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product.NameProduct))
                .ForMember(dest => dest.ValueTotalItem, opt => opt.MapFrom(src => src.Quantity * (src.PriceUnit - src.Discount)));

            CreateMap<SaleModel, Sale>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.IdClient))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.IdBranch))
                .ForMember(dest => dest.Client, opt => opt.Ignore())
                .ForMember(dest => dest.Branch, opt => opt.Ignore())
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens));

            CreateMap<ItemSaleModel, ItemSale>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Product, opt => opt.Ignore());
        }
    }
}
