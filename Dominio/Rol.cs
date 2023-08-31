namespace Dominio;

    public class Rol : BaseEntityA
    {
         public string NameRol { get; set;}
         public ICollection<User> Users { get; set; } = new HashSet<User>();
         public ICollection<UserRol> UserRols { get; set; }
    }
