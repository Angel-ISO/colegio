using Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Persons");


            builder.Property(p => p.Id)
            .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .HasColumnName("Id_Person")
            .HasColumnType("int")
            .IsRequired();


             builder.Property(p => p.Name)
            .HasColumnName("NamePerson")
            .HasColumnType("varchar")
            .HasMaxLength(150)
            .IsRequired();

               builder.Property(p => p.Lastname)
            .HasColumnName("Lastname")
            .HasColumnType("varchar")
            .HasMaxLength(150)
            .IsRequired();

             builder.Property(p => p.BirthDate)
            .HasColumnName("BIrilDate")
            .HasColumnType("date")
            .IsRequired();



        }
    }
}