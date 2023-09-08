using Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace persistencia.Configuration;
public class InscriptionConfiguration : IEntityTypeConfiguration<Inscription>
{
    public void Configure(EntityTypeBuilder<Inscription> builder)
    {
        builder.ToTable("Inscription");

            builder.Property(p => p.Id)
            .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .HasColumnName("Id_Inscriptio")
            .HasColumnType("int")
            .IsRequired();

            builder.Property(p => p.Fecha_Inscription)
            .HasColumnName("fechainscripcion")
            .HasColumnType("date")
            .IsRequired();


            builder.HasOne(u => u.Person)
            .WithMany(a => a.Inscriptions)
            .HasForeignKey(u => u.IdPerson)
            .IsRequired();

              builder.HasOne(u => u.Curso)
            .WithMany(a => a.Inscriptions)
            .HasForeignKey(u => u.Id_Curso)
            .IsRequired();

          
    }
}