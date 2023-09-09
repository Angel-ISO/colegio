namespace ColegioAPI.Helpers;

    public class Autorizacion
    {
        public enum Rols
        {
            Rector,
            Coordinador,
            DirectorDeGrupo,
            Estudiante
        }
        public const Rols Rol_PorDefecto = Rols.Estudiante;
    }