using Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {

            builder.ToTable("Rol");

       
            builder.Property(p => p.Id)
            .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .HasColumnName("Id_Rol")
            .HasColumnType("int")
            .IsRequired();


     
            builder.Property(p => p.NameRol)
            .HasColumnName("NameRol")
            .HasColumnType("varchar")
            .HasMaxLength(200)
            .IsRequired();

    }
}