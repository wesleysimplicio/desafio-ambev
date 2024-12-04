using AutoMapper;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sales.Data.Events;
using Sales.Data.Contexts;
using Sales.Data.Repositories;
using Sales.Data.Seed;
using Sales.Domain.Validators;
using Sales.Domain.Interfaces.Events;
using Sales.WebApi.Middleware;
using Sales.Domain.Interfaces.Repositories;
using Sales.Domain.Mappings;
using Sales.Domain.Services;
using Sales.WebApi.Endpoints;
using Serilog;
using Sales.Domain.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<SaleModelValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<SaleValidator>();


builder.Services.AddDbContext<SalesContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();

builder.Services.AddSingleton<IEventDispatcher>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<IEventDispatcher>>();
    bool useRabbitMq = builder.Configuration.GetValue<bool>("UseRabbitMq");
    return new EventDispatcher(logger, useRabbitMq);
});


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new SaleProfile());
});
var mapper = mapperConfig.CreateMapper();


builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton(mapper);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.AddEndpoints();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SalesContext>();
    context.Database.Migrate();

    await DataSeeder.SeedBranchesAsync(context);

    await DataSeeder.SeedProductsAsync(context);
    
    await DataSeeder.SeedClientsAsync(context);
}

app.UseMiddleware<ErrorMiddleware>();
app.Run();
