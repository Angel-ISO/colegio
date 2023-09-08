
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio;

    public class Person : BaseEntityA
    {

        public User User { get; set; }   // Reference navigation to dependent
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Inscription> Inscriptions { get; set; }
        public ICollection<Curso> Cursos { get; set; }
    }
