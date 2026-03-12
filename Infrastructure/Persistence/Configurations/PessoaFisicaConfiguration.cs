using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PessoaFisicaConfiguration : IEntityTypeConfiguration<PessoaFisica>
{
    public void Configure(EntityTypeBuilder<PessoaFisica> builder)
    {
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(200);
    }
}