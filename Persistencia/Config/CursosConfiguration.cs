using Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace persistencia.Configuration;
public class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("curso");

            builder.Property(p => p.Id)
            .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .HasColumnName("Id_curso")
            .HasColumnType("int")
            .IsRequired();

             builder.Property(p => p.NameCourse)
            .HasColumnName("NameCourse")
            .HasColumnType("varchar")
            .HasMaxLength(150)
            .IsRequired();





            builder.HasOne(u => u.User)
            .WithMany(a => a.Cursos)
            .HasForeignKey(u => u.IdProfe)
            .IsRequired();

            
          
    }
}