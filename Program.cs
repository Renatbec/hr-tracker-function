using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

// Legg til user secrets manuelt
builder.Configuration
	.AddUserSecrets<Program>(optional: true);

// Bygg konfigurasjonen
var configuration = builder.Configuration;

// Hent connection string fra user secrets
var connectionString = configuration["AzureWebTableStorage"];

// Registrer TableClient som singleton
builder.Services.AddSingleton(new TableClient(connectionString, "Boats"));

// Legg til logging
builder.Services.AddLogging(logging =>
{
	logging.ClearProviders();
	logging.AddConsole();
	logging.SetMinimumLevel(LogLevel.Information);
});

// Konfigurer web-applikasjonen
builder.ConfigureFunctionsWebApplication();

builder.Build().Run();
