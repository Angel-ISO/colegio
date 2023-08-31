
namespace Dominio;

    public class User : BaseEntityA
    {
        public string NameUser {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}

        public ICollection<Inscription> Inscriptions { get; set; }
        public ICollection<Curso> Cursos { get; set; }
        public ICollection<Rol> Rols { get; set; } = new HashSet<Rol>();
        public ICollection<UserRol> UserRols { get; set; }
    }
