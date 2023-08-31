using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Dominio;


namespace Persistencia;
public class ColegioContext : DbContext {
    public ColegioContext(DbContextOptions<ColegioContext> options) : base(options) { 

    }
        public DbSet<User> Users { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<UserRol> UserRols { get; set; } 
        public DbSet<Inscription> Inscriptions { get; set; }
        public DbSet<Curso> Cursos { get; set; }

    
       protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}