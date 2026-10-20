using FornecedoresApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FornecedoresApp.Infrastructure.Mappings;

public sealed class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario");
        builder.HasKey(usuario => usuario.Id);

        builder.Property(usuario => usuario.Id).HasColumnType("uniqueidentifier");
        builder.Property(usuario => usuario.Nome).HasMaxLength(100).IsRequired();
        builder.Property(usuario => usuario.Login).HasMaxLength(100).IsRequired();
        builder.Property(usuario => usuario.SenhaHash).HasMaxLength(500).IsRequired();
        builder.Property(usuario => usuario.Ativo).HasColumnType("bit").HasDefaultValue(true).IsRequired();
        builder.Property(usuario => usuario.DataCadastro).HasColumnType("datetime2").IsRequired();

        builder.HasIndex(usuario => usuario.Login).IsUnique();
    }
}
