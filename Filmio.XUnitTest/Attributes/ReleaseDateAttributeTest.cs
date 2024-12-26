using System.ComponentModel.DataAnnotations;
using Filmio.BLL.Attributes;
using FluentAssertions;

namespace Filmio.XUnitTest.Attributes;

public class ReleaseDateAttributeTest
{
    private readonly ReleaseDateAttribute _attribute;

    public ReleaseDateAttributeTest()
    {
        _attribute = new ReleaseDateAttribute();
    }

    [Fact]
    public void ShouldReturnSuccess_WhenReleaseDateIsValid()
    {
        // Arrange
        var validDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var result = _attribute.GetValidationResult(validDate, new ValidationContext(new object()));

        // Assert
        result.Should().Be(ValidationResult.Success);
    }

    [Fact]
    public void ShouldReturnError_WhenReleaseDateIsInTheFuture()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);

        // Act
        var result = _attribute.GetValidationResult(futureDate, new ValidationContext(new object()));

        // Assert
        result.Should().NotBe(ValidationResult.Success);
        result?.ErrorMessage.Should().Be("Release date cannot be in the future");
    }

    [Fact]
    public void ShouldReturnError_WhenValueIsNull()
    {
        // Act
        var result = _attribute.GetValidationResult(null, new ValidationContext(new object()));

        // Assert
        result.Should().NotBe(ValidationResult.Success);
        result?.ErrorMessage.Should().Be("Input parameter cannot be null");
    }

    [Fact]
    public void ShouldReturnSuccess_WhenValueIsNotDateTime()
    {
        // Arrange
        var invalidValue = "Not a date";

        // Act
        var result = _attribute.GetValidationResult(invalidValue, new ValidationContext(new object()));

        // Assert
        result.Should().Be(ValidationResult.Success);
    }
}