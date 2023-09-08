

namespace Dominio;

    public class Inscription : BaseEntityA
    {
        public int IdPerson { get; set; }
        public Person Person { get; set; }
        public DateTime Fecha_Inscription { get; set; }
        public int Id_Curso { get; set; }
        public Curso Curso { get; set; }
    }
