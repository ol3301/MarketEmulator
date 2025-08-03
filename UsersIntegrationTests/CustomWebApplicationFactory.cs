using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using UsersApi.Application.Domain;
using UsersApi.Application.Domain.Entities;
using UsersApi.Application.Infrastructure.Postgres;

namespace UsersIntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Remove(services.First(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<UsersDbContext>)));

            services.AddDbContext<UsersDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            using var db = services.BuildServiceProvider()
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<UsersDbContext>();
            
            db.Users.Add(new UserEntity { Id = 1, Name = "TestUser", Email = "test@test.com"});
            db.SaveChanges();
        });
    }
}
