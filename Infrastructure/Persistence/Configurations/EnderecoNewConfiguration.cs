using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EnderecoNewConfiguration : IEntityTypeConfiguration<EnderecoNew>
{
    public void Configure(EntityTypeBuilder<EnderecoNew> builder)
    {
        builder.ToTable("EnderecosNew");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ClienteId)
            .IsRequired();

        builder.Property(x => x.Cep)
            .IsRequired()
            .HasMaxLength(9);

        builder.Property(x => x.Logradouro)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Numero)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Complemento)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Bairro)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Cidade)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Estado)
            .IsRequired()
            .HasMaxLength(2);

        builder.HasIndex(x => x.ClienteId);
    }
}
