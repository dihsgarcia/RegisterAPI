using Application.Interfaces;
using Application.Services;
using Application.Validators;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using RegisterAPI.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Integrations.Cep;
using Infrastructure.Settings;
using Microsoft.Extensions.Http.Resilience;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<ViaCepSettings>(
    builder.Configuration.GetSection("ExternalServices"));

//Retry Policy
builder.Services
    .AddHttpClient<ViaCepProvider>()
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.MaxRetryAttempts = 3;
        options.Retry.Delay = TimeSpan.FromSeconds(2);

        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(5);
    });

builder.Services.AddScoped<ICepProvider, ViaCepProvider>();
builder.Services.AddScoped<ICepService, CepService>();

builder.Services.AddScoped<IPessoaFisicaService, PessoaFisicaService>();
builder.Services.AddScoped<IPessoaJuridicaService, PessoaJuridicaService>();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

// Controllers
builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<CreatePessoaFisicaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePessoaJuridicaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEnderecoClienteValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<UpdatePessoaFisicaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePessoaJuridicaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateEnderecoClienteValidator>();

builder.Services.AddFluentValidationAutoValidation();

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

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();