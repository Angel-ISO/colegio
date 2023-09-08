namespace Dominio;

    public class Curso : BaseEntityA
    {
        public string NameCourse { get; set; }
        public int IdProfe { get; set; }
        public Person Person { get; set;}

        public ICollection<Inscription> Inscriptions { get; set; }

    }
