using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(x => x.ClienteId);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.RazaoSocial)
            .HasMaxLength(200);

        builder.Property(x => x.Documento)
            .IsRequired()
            .HasMaxLength(14);  
        
        builder.HasIndex(x => x.Documento)
            .IsUnique()
            .HasFilter("[DataExclusao] IS NULL");

        builder.Property(x => x.DataCriacao)
            .IsRequired();
        
        builder.Property(x => x.DataAtualizacao);
        builder.Property(x => x.DataExclusao);
        
        builder.HasMany(c => c.Enderecos)
            .WithOne(e => e.Cliente)
            .HasForeignKey(e => e.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Metadata
            .FindNavigation(nameof(Cliente.Enderecos))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(c => c.Enderecos)
            .HasField("_enderecos");
    }
}
