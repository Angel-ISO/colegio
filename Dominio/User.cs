
namespace Dominio;

    public class User : BaseEntityA
    {

     /*    public int PersonId { get; set; } // Required foreign key property
        public Person Person { get; set; } = null!; // Required reference navigation to principal */
        public string NameUser {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public ICollection<Rol> Rols { get; set; } = new HashSet<Rol>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
        public ICollection<UserRol> UserRols { get; set; }
    }
