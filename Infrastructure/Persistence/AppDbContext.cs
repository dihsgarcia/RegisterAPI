using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<PessoaFisica> PessoasFisicas => Set<PessoaFisica>();

    public DbSet<PessoaJuridica> PessoasJuridicas => Set<PessoaJuridica>();

    public DbSet<Endereco> Enderecos => Set<Endereco>();

    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}