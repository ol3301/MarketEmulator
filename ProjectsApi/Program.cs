using MongoDB.Driver;
using NATS.Net;
using ProjectsApi.Application.Background;
using ProjectsApi.Application.Domain.Interfaces;
using ProjectsApi.Application.EventHandlers;
using ProjectsApi.Application.Infrastructure.MongoDb;
using ProjectsApi.Application.Infrastructure.Nats;
using ProjectsApi.Application.Queries;
using ProjectsApi.Application.Services;
using ProjectsApi.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MongoDbContext>(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("ProjectsDatabase");
    return new MongoDbContext(new MongoClient(connectionString));
});
builder.Services.AddSingleton<NatsEventsConsumer>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("NatsMq")!;
    return new NatsEventsConsumer(new NatsClient(connectionString), provider.GetRequiredService<ILogger<NatsEventsConsumer>>());
});

builder.Services.AddTransient<ProjectQueries>();
builder.Services.AddTransient<UserSettingsQueries>();
builder.Services.AddTransient<ProjectCreatorService>();
builder.Services.AddTransient<UserSettingsCreatorService>();
builder.Services.AddTransient<IEventHandlerFactory, EventHandlerFactory>();
builder.Services.AddTransient<UserCreatedEventHandler>();
builder.Services.AddTransient<SubscriptionUpdatedEventHandler>();

builder.Services.AddHostedService<IntegrationEventsSyncTask>();

var app = builder.Build();

app.MapUserProjectsRoutes()
    .MapUserSettingsRoutes()
    .MapGet("/api/popularIndicators/{subscriptionType:int}", async (int subscriptionType, ProjectQueries queriesService) =>
    {
        return Results.Ok(await queriesService.GetMostUsedIndecatorsAsync(subscriptionType));
    });;

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

