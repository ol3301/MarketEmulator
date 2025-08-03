using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Infrastructure.Postgres;

namespace UsersTests;

public class AutoMoqDataAttribute() : AutoDataAttribute(() =>
{
    var fixture = new Fixture().Customize(new AutoMoqCustomization());

    var dbContext = new UsersDbContext(new DbContextOptionsBuilder<UsersDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options);

    fixture.Inject(dbContext);

    return fixture;
})
{
}