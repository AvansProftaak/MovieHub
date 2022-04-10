using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MovieHub.ViewModels;
using Xunit;

namespace MovieHubTests;

public class RoleTest
{

    [Fact]
    public void AddRoleFieldCannotBeBlank()
    {
        // Arrange
        var validationResults = new List<ValidationResult>();

        var sut = new AddRoleViewmodel()
        {
            RoleName = ""
        };
        var ctx = new ValidationContext(sut, null, null);

        // Act
        Validator.TryValidateObject(sut, ctx, validationResults, true);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults,
            validationResult => validationResult.ErrorMessage.Contains("The Role field is required"));

    }

    [Fact]
    public void EditRoleNameFieldCannotBeBlank()
    {
        // Arrange
        var validationResults = new List<ValidationResult>();

        var sut = new EditRoleViewModel()
        {
            Id = "ID-1",
            RoleName = ""
        };
        var ctx = new ValidationContext(sut, null, null);

        // Act
        Validator.TryValidateObject(sut, ctx, validationResults, true);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults,
            validationResult => validationResult.ErrorMessage.Contains("Role Name is required"));

    }
    
}