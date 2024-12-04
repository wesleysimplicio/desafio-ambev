using FluentValidation;
using Sales.Domain.Entities;

namespace Sales.Domain.Validators
{
    public class ItemSaleValidator : AbstractValidator<ItemSale>
    {
        public ItemSaleValidator()
        {
            RuleFor(iv => iv.Quantity)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.")
                .LessThanOrEqualTo(20).WithMessage("Não é permitido vender mais de 20 itens iguais.");

            RuleFor(iv => iv.Quantity)
                .LessThan(4)
                .When(iv => iv.Discount > 0)
                .WithMessage("Compras abaixo de 4 itens não podem ter desconto.");

            RuleFor(iv => iv.Discount)
                .Cascade(CascadeMode.Stop)
                .Must((item, discount) => ValidarDesconto(item)).WithMessage("O desconto aplicado é inválido.");

        }

        private bool ValidarDesconto(ItemSale item)
        {
            if (item.Quantity < 4 && item.Discount > 0)
                return false;

            if (item.Quantity >= 4 && item.Quantity < 10)
                return item.Discount <= item.PriceUnit * 0.10M;

            if (item.Quantity >= 10 && item.Quantity <= 20)
                return item.Discount <= item.PriceUnit * 0.20M;

            return true;
        }
    }
}
