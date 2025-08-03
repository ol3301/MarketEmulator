using Microsoft.EntityFrameworkCore;
using NATS.Net;
using UsersApi.Application.Domain.Interfaces;
using UsersApi.Application.Infrastructure.Nats;
using UsersApi.Application.Infrastructure.Postgres;
using UsersApi.Application.Queries;
using UsersApi.Application.Services;
using UsersApi.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UsersDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("UsersDatabase"));
});

builder.Services.AddSingleton<IIntegrationEventPublisher>(_ =>
{
    return new NatsEventPublisher(new NatsClient(builder.Configuration.GetConnectionString("NatsMq")!), "integration-events");
});

builder.Services.AddTransient<UserCreatorAppService>();
builder.Services.AddTransient<UserSubscriptionService>();
builder.Services.AddTransient<UserQueries>();

var app = builder.Build();

app.MapUsersRoutes();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        db.Database.Migrate();
    }
}

app.Run();
namespace UsersApi
{
    public partial class Program { }
}//for integration tests