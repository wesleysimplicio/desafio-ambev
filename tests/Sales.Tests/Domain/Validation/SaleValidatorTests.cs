using FluentValidation.TestHelper;
using Sales.Domain.Entities;
using Sales.Domain.Validators;
using Xunit;

namespace Sales.Tests.Domain.Validators
{
    public class SaleValidatorTests
    {
        [Fact]
        public void SaleValidator_Should_Validate_ValueTotal_GreaterThanZero()
        {
            // Arrange
            var sale = new Sale
            {
                ValueTotal = 0
            };
            var validator = new SaleValidator();

            // Act
            var result = validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.ValueTotal)
                  .WithErrorMessage("O valor total da venda deve ser maior que zero.");
        }

        [Fact]
        public void SaleValidator_Should_Pass_When_ValueTotal_Is_Valid()
        {
            // Arrange
            var sale = new Sale
            {
                ValueTotal = 100.0m
            };
            var validator = new SaleValidator();

            // Act
            var result = validator.TestValidate(sale);

            // Assert
            result.ShouldNotHaveValidationErrorFor(s => s.ValueTotal);
        }

        [Fact]
        public void SaleValidator_Should_Validate_ItemSale_Using_ItemSaleValidator()
        {
            // Arrange
            var sale = new Sale
            {
                Itens = new List<ItemSale>
                {
                    new ItemSale
                    {
                        Quantity = 0, // Invalid quantity
                        PriceUnit = 10.0m
                    }
                },
                ValueTotal = 100.0m
            };

            var validator = new SaleValidator();

            // Act
            var result = validator.TestValidate(sale);

            // Assert
            var itemValidationResult = new ItemSaleValidator().TestValidate(sale.Itens.First());
            itemValidationResult.ShouldHaveValidationErrorFor(i => i.Quantity);
        }
    }
}
