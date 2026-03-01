using Application.Configurations;
using Application.Interfaces;
using Application.Services;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<ViaCepSettings>(
    builder.Configuration.GetSection("ExternalServices"));

builder.Services.AddHttpClient<IViaCepService, ViaCepService>();
builder.Services.AddScoped<IPessoaFisicaRepository, PessoaFisicaRepository>();
builder.Services.AddScoped<IPessoaFisicaService, PessoaFisicaService>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Register API",
        Version = "v1"
    });
});

var app = builder.Build();

// Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();