using FluentValidation.TestHelper;
using Sales.Domain.Models;
using Sales.Domain.Validators;
using Xunit;

namespace Sales.Tests.Domain.Validators
{
    public class SaleModelValidatorTests
    {
        [Fact]
        public void SaleModelValidator_Should_Validate_Required_Fields()
        {
            // Arrange
            var saleModel = new SaleModel
            {
                Branch = string.Empty, // Invalid
                NumberSale = null,     // Invalid
                ValueTotal = 0,        // Invalid
                Client = string.Empty, // Invalid
                Itens = new List<ItemSaleModel>() // Valid empty list for now
            };

            var validator = new SaleModelValidator();

            // Act
            var result = validator.TestValidate(saleModel);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.Branch).WithErrorMessage("A filial é obrigatória.");
            result.ShouldHaveValidationErrorFor(s => s.NumberSale).WithErrorMessage("O número da venda é obrigatório.");
            result.ShouldHaveValidationErrorFor(s => s.ValueTotal).WithErrorMessage("O valor total da venda deve ser maior que zero.");
            result.ShouldHaveValidationErrorFor(s => s.Client).WithErrorMessage("O cliente é obrigatório.");
        }

        [Fact]
        public void SaleModelValidator_Should_Validate_NumberSale_MaxLength()
        {
            // Arrange
            var saleModel = new SaleModel
            {
                Branch = "Valid Branch",
                NumberSale = new string('A', 51), // Exceeds max length
                ValueTotal = 100,
                Client = "Valid Client",
                Itens = new List<ItemSaleModel>()
            };

            var validator = new SaleModelValidator();

            // Act
            var result = validator.TestValidate(saleModel);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.NumberSale).WithErrorMessage("O número da venda não pode exceder 50 caracteres.");
        }

        [Fact]
        public void SaleModelValidator_Should_Validate_ItemSale_Using_ItemSaleModelValidator()
        {
            // Arrange
            var saleModel = new SaleModel
            {
                Branch = "Valid Branch",
                NumberSale = "12345",
                ValueTotal = 100,
                Client = "Valid Client",
                Itens = new List<ItemSaleModel>
                {
                    new ItemSaleModel
                    {
                        PriceUnit = 0,     // Invalid
                        Quantity = 0,      // Invalid
                        Product = string.Empty // Invalid
                    }
                }
            };

            var validator = new SaleModelValidator();

            // Act
            var result = validator.TestValidate(saleModel);

            // Assert
            var itemValidationResult = new ItemSaleModelValidator().TestValidate(saleModel.Itens.First());
            itemValidationResult.ShouldHaveValidationErrorFor(i => i.PriceUnit).WithErrorMessage("O preço unitário deve ser maior que zero.");
            itemValidationResult.ShouldHaveValidationErrorFor(i => i.Quantity).WithErrorMessage("A quantidade deve ser maior que zero.");
            itemValidationResult.ShouldHaveValidationErrorFor(i => i.Product).WithErrorMessage("O nome do produto é obrigatório.");
        }
    }
}
