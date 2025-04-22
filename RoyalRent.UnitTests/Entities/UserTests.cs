using FluentAssertions;
using RoyalRent.Domain.Entities;

namespace RoyalRent.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void User_ShouldReturnUserSucessfully()
    {
        var nowCreatedOn = DateTime.UtcNow.ToUniversalTime();
        var nowUpdatedOn = DateTime.UtcNow.ToUniversalTime();

        var user = new User("Jonh Doe", "12345678901", "jonh_doe@test.com", "12345678901", 'M');

        user.CreatedOn.Should().BeCloseTo(nowCreatedOn, TimeSpan.FromSeconds(2));
        user.UpdatedOn.Should().BeCloseTo(nowUpdatedOn, TimeSpan.FromSeconds(2));

        Assert.NotNull(user);
        Assert.Equal("Jonh Doe", user.Name);

        Assert.IsType<char>(user.Gender);
        Assert.IsType<string>(user.Telephone);
        Assert.IsType<Guid>(user.Id);
        Assert.IsType<DateTime>(user.CreatedOn);
        Assert.IsType<DateTime>(user.UpdatedOn);
    }
}
