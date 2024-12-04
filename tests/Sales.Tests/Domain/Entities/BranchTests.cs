using Sales.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Sales.Tests.Domain.Entities
{
    public class BranchTests
    {
        [Fact]
        public void Branch_Should_Have_Valid_Default_Values()
        {
            // Arrange
            var branch = new Branch();

            // Assert
            Assert.NotNull(branch.NameBranch);
            Assert.Equal(string.Empty, branch.NameBranch);
            Assert.Null(branch.Address);
            Assert.Null(branch.City);
            Assert.Null(branch.State);
            Assert.Empty(branch.Sales);
        }

        [Fact]
        public void Branch_Should_Validate_NameBranch_Is_Required()
        {
            // Arrange
            var branch = new Branch { NameBranch = null! };

            // Act
            var validationResults = ValidateModel(branch);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(branch.NameBranch)) && v.ErrorMessage!.Contains("required"));
        }

        [Fact]
        public void Branch_Should_Validate_MaxLength_Constraints()
        {
            // Arrange
            var branch = new Branch
            {
                NameBranch = new string('A', 256), // Exceeds MaxLength
                Address = new string('B', 256),    // Exceeds MaxLength
                City = new string('C', 101),      // Exceeds MaxLength
                State = new string('D', 51)       // Exceeds MaxLength
            };

            // Act
            var validationResults = ValidateModel(branch);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(branch.NameBranch)) && v.ErrorMessage!.Contains("maximum length"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(branch.Address)) && v.ErrorMessage!.Contains("maximum length"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(branch.City)) && v.ErrorMessage!.Contains("maximum length"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(branch.State)) && v.ErrorMessage!.Contains("maximum length"));
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
