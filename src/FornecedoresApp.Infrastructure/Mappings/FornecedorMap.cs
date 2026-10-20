using FornecedoresApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FornecedoresApp.Infrastructure.Mappings;

public sealed class FornecedorMap : IEntityTypeConfiguration<Fornecedor>
{
    private const string CollationEmailCaseInsensitive = "SQL_Latin1_General_CP1_CI_AS";

    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        builder.ToTable("Fornecedor");
        builder.HasKey(fornecedor => fornecedor.Id);

        builder.Property(fornecedor => fornecedor.Id).HasColumnType("uniqueidentifier");
        builder.Property(fornecedor => fornecedor.RazaoSocial).HasMaxLength(150).IsRequired();
        builder.Property(fornecedor => fornecedor.NomeFantasia).HasMaxLength(150);
        builder.Property(fornecedor => fornecedor.Cnpj).HasColumnType("varchar(14)").HasMaxLength(14).IsRequired();
        builder.Property(fornecedor => fornecedor.Email)
            .HasMaxLength(150)
            .UseCollation(CollationEmailCaseInsensitive)
            .IsRequired();
        builder.Property(fornecedor => fornecedor.Endereco).HasMaxLength(250).IsRequired();
        builder.Property(fornecedor => fornecedor.Telefone).HasMaxLength(20);
        builder.Property(fornecedor => fornecedor.Ativo).HasColumnType("bit").HasDefaultValue(true).IsRequired();
        builder.Property(fornecedor => fornecedor.DataCadastro).HasColumnType("datetime2").IsRequired();

        builder.HasIndex(fornecedor => fornecedor.Cnpj).IsUnique();
        builder.HasIndex(fornecedor => fornecedor.Email).IsUnique();
    }
}
