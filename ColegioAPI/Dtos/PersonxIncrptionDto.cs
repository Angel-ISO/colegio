namespace ColegioAPI.Dtos;

    public class PersonxIncriptionDto
    {
        public string Id {get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string BirthDate { get; set; }
        public List<InscriptionDto> Inscripciones { get; set; }
    }
