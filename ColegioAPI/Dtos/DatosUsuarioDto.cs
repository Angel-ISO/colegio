using System.Text.Json.Serialization;

namespace ColegioAPI.Dtos;

    public class DatosUsuarioDto
    {
        public string Mensaje { get; set; }
        public bool EstaAutenticado { get; set; }
        public string UserName { get; set; }
         public List<string>  Rols { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }


    [JsonIgnore] // esta sentencia evita que estos atributos se muestren en las peticiones
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
    }