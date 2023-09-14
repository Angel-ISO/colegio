using Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");


            builder.Property(p => p.Id)
            .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .HasColumnName("Id_User")
            .HasColumnType("int")
            .IsRequired();


             builder.Property(p => p.Username)
            .HasColumnName("NameUser")
            .HasColumnType("varchar")
            .HasMaxLength(150)
            .IsRequired();



            builder
            .HasMany(p => p.Rols)
            .WithMany(p => p.Users)
            .UsingEntity<UserRol>(
                j => j
                    .HasOne(pt => pt.Rol)
                    .WithMany(t => t.UserRols)
                    .HasForeignKey(pt => pt.RolId),
                j => j
                    .HasOne(pt => pt.User)
                    .WithMany(p => p.UserRols)
                    .HasForeignKey(pt => pt.UserId),
                j =>
                {
                    j.HasKey(t => new { t.UserId, t.RolId });
                });

        }
    }
}