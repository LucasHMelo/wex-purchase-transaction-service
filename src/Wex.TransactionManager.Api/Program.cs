using Serilog;
using Wex.TransactionManager.Api.Configurations;
using Wex.TransactionManager.Application.Interfaces;
using Wex.TransactionManager.Infrastructure.Clients;

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

// Add services to the container.
builder.Services.AddHttpClient<ITreasuryApiClient, TreasuryApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TreasuryApi:BaseUrl"] 
                               ?? throw new InvalidOperationException("TreasuryApi:BaseUrl não configurado."));
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services
    .AddAppConections(builder.Configuration)
    .AddUseCases()
    .AddAndConfigureControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }