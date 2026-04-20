using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Serilog;
using StackExchange.Redis;
using Wex.TransactionManager.Api.Configurations;
using Wex.TransactionManager.Application.UseCases.Transactions.CreateTransactions;
using Wex.TransactionManager.Application.UseCases.Transactions.GetTransactions;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, configuration) =>
    configuration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
        .Enrich.FromLogContext()
        .Enrich.WithThreadId()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {SourceContext}: {Message:lj} {Properties:j}{NewLine}{Exception}"));

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
if(string.IsNullOrEmpty(redisConnectionString)) 
    throw new ArgumentNullException("ConnectionString.Redis");

builder.Services.AddSingleton<IConnectionMultiplexer>(
    
    ConnectionMultiplexer.Connect(redisConnectionString)
);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "wex-transactions:";
});

builder.Services
    .AddPollyConfiguration(builder.Configuration)
    .AddAppConections(builder.Configuration)
    .AddUseCases()
    .AddAndConfigureControllers();


builder.Services.AddFusionCache()
    .WithDefaultEntryOptions(new FusionCacheEntryOptions
    {
        Duration = TimeSpan.FromMinutes(5),
        DistributedCacheDuration = TimeSpan.FromHours(24),

        IsFailSafeEnabled = true,
        FailSafeMaxDuration = TimeSpan.FromHours(1),

        FactorySoftTimeout = TimeSpan.FromSeconds(2),
        FactoryHardTimeout = TimeSpan.FromSeconds(5),

        EagerRefreshThreshold = 0.7f
    })
    .WithDistributedCache(sp =>
        sp.GetRequiredService<IDistributedCache>())
    .WithSerializer( 
        new FusionCacheSystemTextJsonSerializer()
    );

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
public partial class Program { }