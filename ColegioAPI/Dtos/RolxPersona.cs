

using Dominio;

namespace InsidenceAPI.Dtos;

    public class RolxPersonDto
    {
         public string Su_Rol { get; set; }
        public string Descripcion_De_Rol {get; set; }   
        public List<Person> Personas {get; set;}

    }