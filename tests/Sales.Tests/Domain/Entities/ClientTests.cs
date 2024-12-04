using Sales.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Sales.Tests.Domain.Entities
{
    public class ClientTests
    {
        [Fact]
        public void Client_Should_Have_Valid_Default_Values()
        {
            // Arrange
            var client = new Client();

            // Assert
            Assert.NotNull(client.NameClient);
            Assert.Equal(string.Empty, client.NameClient);
            Assert.Null(client.Email);
            Assert.Null(client.Phone);
            Assert.Empty(client.Sales);
        }

        [Fact]
        public void Client_Should_Validate_NameClient_Is_Required()
        {
            // Arrange
            var client = new Client { NameClient = null! };

            // Act
            var validationResults = ValidateModel(client);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(client.NameClient)) && v.ErrorMessage!.Contains("required"));
        }

        [Fact]
        public void Client_Should_Validate_MaxLength_Constraints()
        {
            // Arrange
            var client = new Client
            {
                NameClient = new string('A', 256), // Exceeds MaxLength
                Email = new string('B', 256),      // Exceeds MaxLength
                Phone = new string('C', 21)       // Exceeds MaxLength
            };

            // Act
            var validationResults = ValidateModel(client);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(client.NameClient)) && v.ErrorMessage!.Contains("maximum length"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(client.Email)) && v.ErrorMessage!.Contains("maximum length"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(client.Phone)) && v.ErrorMessage!.Contains("maximum length"));
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
