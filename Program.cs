using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

// Konfigurer web-applikasjonen
builder.ConfigureFunctionsWebApplication();

builder.Build().Run();
