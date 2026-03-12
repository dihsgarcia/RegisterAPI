using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PessoaJuridicaConfiguration : IEntityTypeConfiguration<PessoaJuridica>
{
    public void Configure(EntityTypeBuilder<PessoaJuridica> builder)
    {
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.RazaoSocial)
            .IsRequired()
            .HasMaxLength(200);
    }
}