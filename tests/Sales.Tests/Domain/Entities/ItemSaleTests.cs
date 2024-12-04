using Sales.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Sales.Tests.Domain.Entities
{
    public class ItemSaleTests
    {
        [Fact]
        public void ItemSale_Should_Calculate_ValueTotal_Correctly()
        {
            // Arrange
            var itemSale = new ItemSale
            {
                Quantity = 5,
                PriceUnit = 10.0m,
                Discount = 2.0m
            };

            // Act
            var valueTotal = itemSale.ValueTotal;

            // Assert
            Assert.Equal(40.0m, valueTotal);
        }

        [Fact]
        public void ItemSale_Should_Validate_Required_Fields()
        {
            // Arrange
            var itemSale = new ItemSale
            {
                SaleId = 0,        // Invalid SaleId
                ProductId = 0,     // Invalid ProductId
                Quantity = 0,      // Invalid Quantity
                PriceUnit = 0.0m   // Invalid PriceUnit
            };

            // Act
            var validationResults = ValidateModel(itemSale);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(itemSale.SaleId)) && v.ErrorMessage!.Contains("required"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(itemSale.ProductId)) && v.ErrorMessage!.Contains("required"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(itemSale.Quantity)) && v.ErrorMessage!.Contains("required"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(itemSale.PriceUnit)) && v.ErrorMessage!.Contains("required"));
        }

        [Fact]
        public void ItemSale_Should_Validate_Default_Discount_Is_Zero()
        {
            // Arrange
            var itemSale = new ItemSale();

            // Act
            var discount = itemSale.Discount;

            // Assert
            Assert.Equal(0.0m, discount);
        }

        private static List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
            return validationResults;
        }
    }
}
