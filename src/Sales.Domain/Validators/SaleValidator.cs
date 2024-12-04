using FluentValidation;
using Sales.Domain.Entities;

namespace Sales.Domain.Validators
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleForEach(v => v.Itens).SetValidator(new ItemSaleValidator());

            RuleFor(v => v.ValueTotal)
                .GreaterThan(0).WithMessage("O valor total da venda deve ser maior que zero.");
        }
    }
}
