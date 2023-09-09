using ColegioAPI.Dtos;
using Dominio;
using AutoMapper;
using persistencia.Configuration;




namespace InsidenceAPI.Profiles;

    public class MappingPofiles : Profile
    {
      public MappingPofiles(){
          CreateMap<Rol, RolDto>().ReverseMap().ForMember(o => o.Users,d => d.Ignore());
          CreateMap<Curso, CursoDto>().ReverseMap();
          CreateMap<Person, PersonDto>().ReverseMap();
          CreateMap<Inscription, InscriptionDto>().ReverseMap();


          //herencia con  query string 

          CreateMap<Person, PersonXcursoDto>().ReverseMap();
          CreateMap<Person, PersonxIncriptionDto>().ReverseMap();
      }
    }