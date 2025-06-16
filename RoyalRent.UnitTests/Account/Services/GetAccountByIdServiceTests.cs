using Moq;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Accounts.Services;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.UnitTests.Account.Services;

public class GetUserServiceTests
{
    [Fact]
    public async Task GetUserBasicInformation_ShouldReturnUserInformations()
    {
        var user = new User("Jonh Doe", "12345678901", "jonh_doe@test.com", "12345678901", 'M');

        var mockRepo = new Mock<IAccountRepository>();
        var mockAuthProvider = new Mock<IAuthenticationProvider>();
        mockRepo.Setup(repo => repo.GetUserBasicInformationById(user.Id)).ReturnsAsync(user);

        var service = new AccountQueryService(mockAuthProvider.Object, mockRepo.Object);

        var result = await service.ExecuteGetByIdAsync(user.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("Jonh Doe", user.Name);
    }
}
