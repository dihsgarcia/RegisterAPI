using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PessoaFisicaConfiguration : IEntityTypeConfiguration<PessoaFisica>
{
    public void Configure(EntityTypeBuilder<PessoaFisica> builder)
    {
        builder.ToTable("PessoasFisicas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(x => x.Cpf)
            .IsUnique();

        builder.HasOne(x => x.Endereco)
            .WithMany()
            .HasForeignKey(x => x.EnderecoId);
    }
}