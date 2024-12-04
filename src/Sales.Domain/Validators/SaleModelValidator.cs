using FluentValidation;
using Sales.Domain.Models;

namespace Sales.Domain.Validators
{
    public class SaleModelValidator : AbstractValidator<SaleModel>
    {
        public SaleModelValidator()
        {

            RuleFor(v => v.Branch)
                .NotEmpty().WithMessage("A filial é obrigatória.");

            RuleFor(v => v.NumberSale)
                .NotEmpty().WithMessage("O número da venda é obrigatório.")
                .MaximumLength(50).WithMessage("O número da venda não pode exceder 50 caracteres.");

            RuleFor(v => v.ValueTotal)
                .GreaterThan(0).WithMessage("O valor total da venda deve ser maior que zero.");

            RuleFor(v => v.Client)
              .NotEmpty().WithMessage("O cliente é obrigatório.");

            RuleForEach(v => v.Itens).SetValidator(new ItemSaleModelValidator());
        }
    }

    public class ItemSaleModelValidator : AbstractValidator<ItemSaleModel>
    {
        public ItemSaleModelValidator()
        {
            RuleFor(i => i.PriceUnit)
                .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");

            RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

            RuleFor(i => i.Product)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.");

        }
    }
}
