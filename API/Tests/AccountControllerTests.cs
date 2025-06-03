using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NutriTrack.Controllers;
using NutriTrack.Data;
using NutriTrack.DTO.User;
using NutriTrack.Entities;
using NutriTrack.Entity.Enums;
using NutriTrack.Services;
using NutriTrack.Services.Interfaces;
using Xunit;

namespace NutriTrack.Tests;

public class AccountControllerTests
{
    [Fact]
    public async Task GetActivityLevels_ReturnsAllActivityLevels()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestActivityLevelsDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        // Add test activity levels
        var activityLevels = new[]
        {
            new ActivityLevel { Id = 1, Name = "Sedentary", Ratio = 1200 },
            new ActivityLevel { Id = 2, Name = "Lightly Active", Ratio = 1375 },
            new ActivityLevel { Id = 3, Name = "Moderately Active", Ratio = 1550 }
        };

        await context.ActivityLevels.AddRangeAsync(activityLevels);
        await context.SaveChangesAsync();

        // Mock dependencies
        var userStore = new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
        var mockTokenService = new Mock<TokenService>(userManager.Object, Mock.Of<IConfiguration>());
        var mockUserService = new Mock<UserService>(context, userManager.Object, Mock.Of<IHttpContextAccessor>());
        var mockEmailSender = new Mock<IEmailSender>();
        var mockConfiguration = new Mock<IConfiguration>();

        var controller = new AccountController(
            context,
            userManager.Object,
            mockTokenService.Object,
            mockUserService.Object,
            mockEmailSender.Object,
            mockConfiguration.Object);

        // Act
        var result = await controller.GetActivityLevels();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var activityLevelsList = Assert.IsType<List<ActivityLevelResponse>>(okResult.Value);

        Assert.Equal(3, activityLevelsList.Count);
        Assert.Contains(activityLevelsList, al => al.Name == "Sedentary");
        Assert.Contains(activityLevelsList, al => al.Name == "Lightly Active");
        Assert.Contains(activityLevelsList, al => al.Name == "Moderately Active");
    }

    [Fact]
    public async Task GetGoalTypes_ReturnsAllGoalTypes()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestGoalTypesDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        // Add test goal types
        var goalTypes = new[]
        {
            new GoalType { Id = 1, Name = "Weight Loss", Percent = 85 },
            new GoalType { Id = 2, Name = "Maintenance", Percent = 100 },
            new GoalType { Id = 3, Name = "Weight Gain", Percent = 115 }
        };

        await context.GoalTypes.AddRangeAsync(goalTypes);
        await context.SaveChangesAsync();

        // Mock dependencies
        var userStore = new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
        var mockTokenService = new Mock<TokenService>(userManager.Object, Mock.Of<IConfiguration>());
        var mockUserService = new Mock<UserService>(context, userManager.Object, Mock.Of<IHttpContextAccessor>());
        var mockEmailSender = new Mock<IEmailSender>();
        var mockConfiguration = new Mock<IConfiguration>();

        var controller = new AccountController(
            context,
            userManager.Object,
            mockTokenService.Object,
            mockUserService.Object,
            mockEmailSender.Object,
            mockConfiguration.Object);

        // Act
        var result = await controller.GetGoalTypes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var goalTypesList = Assert.IsType<List<GoalTypeResponse>>(okResult.Value);

        Assert.Equal(3, goalTypesList.Count);
        Assert.Contains(goalTypesList, gt => gt.Name == "Weight Loss");
        Assert.Contains(goalTypesList, gt => gt.Name == "Maintenance");
        Assert.Contains(goalTypesList, gt => gt.Name == "Weight Gain");
    }

    [Fact]
    public async Task CheckAvailability_WithAvailableEmailAndUsername_ReturnsAvailable()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestCheckAvailabilityDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        // Mock UserManager
        var userStore = new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

        userManager.Setup(x => x.FindByEmailAsync("available@test.com"))
            .ReturnsAsync((User)null);
        userManager.Setup(x => x.FindByNameAsync("availableuser"))
            .ReturnsAsync((User)null);

        // Mock other dependencies
        var mockTokenService = new Mock<TokenService>(userManager.Object, Mock.Of<IConfiguration>());
        var mockUserService = new Mock<UserService>(context, userManager.Object, Mock.Of<IHttpContextAccessor>());
        var mockEmailSender = new Mock<IEmailSender>();
        var mockConfiguration = new Mock<IConfiguration>();

        var controller = new AccountController(
            context,
            userManager.Object,
            mockTokenService.Object,
            mockUserService.Object,
            mockEmailSender.Object,
            mockConfiguration.Object);

        // Act
        var result = await controller.CheckAvailability("available@test.com", "availableuser");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        dynamic value = okResult.Value;
        Assert.True(value.GetType().GetProperty("isEmailAvailable").GetValue(value));
        Assert.True(value.GetType().GetProperty("isUsernameAvailable").GetValue(value));
    }

    [Fact]
    public async Task CheckAvailability_WithTakenEmailAndUsername_ReturnsNotAvailable()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestCheckAvailabilityTakenDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        // Create existing user data
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "taken@test.com",
            UserName = "takenuser"
        };

        // Mock UserManager to return existing users
        var userStore = new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

        userManager.Setup(x => x.FindByEmailAsync("taken@test.com"))
            .ReturnsAsync(existingUser);
        userManager.Setup(x => x.FindByNameAsync("takenuser"))
            .ReturnsAsync(existingUser);

        // Mock other dependencies
        var mockTokenService = new Mock<TokenService>(userManager.Object, Mock.Of<IConfiguration>());
        var mockUserService = new Mock<UserService>(context, userManager.Object, Mock.Of<IHttpContextAccessor>());
        var mockEmailSender = new Mock<IEmailSender>();
        var mockConfiguration = new Mock<IConfiguration>();

        var controller = new AccountController(
            context,
            userManager.Object,
            mockTokenService.Object,
            mockUserService.Object,
            mockEmailSender.Object,
            mockConfiguration.Object);

        // Act
        var result = await controller.CheckAvailability("taken@test.com", "takenuser");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        dynamic value = okResult.Value;
        Assert.False(value.GetType().GetProperty("isEmailAvailable").GetValue(value));
        Assert.False(value.GetType().GetProperty("isUsernameAvailable").GetValue(value));
    }

    [Fact]
    public async Task ResetPassword_WithInvalidToken_ReturnsBadRequest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestResetPasswordDb")
            .Options;

        await using var context = new ApplicationDbContext(options);

        // Create a test user
        var testUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "test@example.com",
            UserGender = Gender.Male,
            DateOfBirth = DateTime.Now.AddYears(-25),
            Height = 175
        };

        // Mock UserManager
        var userStore = new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

        userManager.Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(testUser);

        // Setup ResetPasswordAsync to return failure for invalid token
        userManager.Setup(x => x.ResetPasswordAsync(testUser, 
                "invalid-token", 
                "NewPassword123!"))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Code = "InvalidToken",
                Description = "Invalid token"
            }));

        // Mock other dependencies
        var mockTokenService = new Mock<TokenService>(userManager.Object, Mock.Of<IConfiguration>());
        var mockUserService = new Mock<UserService>(context, userManager.Object, Mock.Of<IHttpContextAccessor>());
        var mockEmailSender = new Mock<IEmailSender>();
        var mockConfiguration = new Mock<IConfiguration>();

        var controller = new AccountController(
            context,
            userManager.Object,
            mockTokenService.Object,
            mockUserService.Object,
            mockEmailSender.Object,
            mockConfiguration.Object);

        var request = new ResetPasswordRequest
        {
            Email = "test@example.com",
            Token = "invalid-token",
            NewPassword = "NewPassword123!"
        };

        // Act
        var result = await controller.ResetPassword(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

        // Fix: Check for IEnumerable<IdentityError> instead of IdentityError[]
        var errors = Assert.IsAssignableFrom<IEnumerable<IdentityError>>(badRequestResult.Value);
        var errorsList = errors.ToList();

        Assert.Single(errorsList);
        Assert.Equal("InvalidToken", errorsList[0].Code);
        Assert.Equal("Invalid token", errorsList[0].Description);
    }
}