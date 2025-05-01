using Moq;
using RoyalRent.Application.Accounts;
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
        mockRepo.Setup(repo => repo.GetUserBasicInformationById(user.Id)).ReturnsAsync(user);

        var service = new GetUserService(mockRepo.Object);

        var result = await service.ExecuteGetByIdAsync(user.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("Jonh Doe", user.Name);
    }
}
