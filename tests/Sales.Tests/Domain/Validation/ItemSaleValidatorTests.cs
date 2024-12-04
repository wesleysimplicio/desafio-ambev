using FluentValidation.TestHelper;
using Sales.Domain.Entities;
using Sales.Domain.Validators;
using Xunit;

namespace Sales.Tests.Domain.Validators
{
    public class ItemSaleValidatorTests
    {
        [Fact]
        public void ItemSaleValidator_Should_Validate_Quantity_Rules()
        {
            // Arrange
            var validator = new ItemSaleValidator();

            // Act & Assert
            var item1 = new ItemSale { Quantity = 0 };
            validator.TestValidate(item1).ShouldHaveValidationErrorFor(i => i.Quantity)
                .WithErrorMessage("A quantidade deve ser maior que zero.");

            var item2 = new ItemSale { Quantity = 21 };
            validator.TestValidate(item2).ShouldHaveValidationErrorFor(i => i.Quantity)
                .WithErrorMessage("Não é permitido vender mais de 20 itens iguais.");
        }

        [Fact]
        public void ItemSaleValidator_Should_Validate_Discount_And_Quantity_Relationship()
        {
            // Arrange
            var validator = new ItemSaleValidator();

            // Act & Assert
            var item = new ItemSale { Quantity = 3, Discount = 5 };
            validator.TestValidate(item).ShouldHaveValidationErrorFor(i => i.Quantity)
                .WithErrorMessage("Compras abaixo de 4 itens não podem ter desconto.");
        }

        [Theory]
        [InlineData(4, 10, true)] // Valid: Discount <= 10% of PriceUnit
        [InlineData(4, 15, false)] // Invalid: Discount > 10% of PriceUnit
        [InlineData(10, 20, true)] // Valid: Discount <= 20% of PriceUnit
        [InlineData(10, 25, false)] // Invalid: Discount > 20% of PriceUnit
        public void ItemSaleValidator_Should_Validate_ValidarDesconto(int quantity, decimal discount, bool isValid)
        {
            // Arrange
            var validator = new ItemSaleValidator();
            var item = new ItemSale
            {
                Quantity = quantity,
                Discount = discount,
                PriceUnit = 100
            };

            // Act
            var result = validator.TestValidate(item);

            // Assert
            if (isValid)
            {
                result.ShouldNotHaveValidationErrorFor(i => i.Discount);
            }
            else
            {
                result.ShouldHaveValidationErrorFor(i => i.Discount)
                    .WithErrorMessage("O desconto aplicado é inválido.");
            }
        }
    }
}
