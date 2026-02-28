using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PessoaJuridicaConfiguration : IEntityTypeConfiguration<PessoaJuridica>
{
    public void Configure(EntityTypeBuilder<PessoaJuridica> builder)
    {
        builder.ToTable("PessoasJuridicas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RazaoSocial)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Cnpj)
            .IsRequired()
            .HasMaxLength(18);

        builder.HasIndex(x => x.Cnpj)
            .IsUnique();

        builder.HasOne(x => x.Endereco)
            .WithMany()
            .HasForeignKey(x => x.EnderecoId);
    }
}