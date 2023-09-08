using InsidenceAPI.Dtos;
using Dominio;
using AutoMapper;
using persistencia.Configuration;




namespace InsidenceAPI.Profiles;

    public class MappingPofiles : Profile
    {
      public MappingPofiles(){
        CreateMap<Person, PersonDto>().ReverseMap();
        CreateMap<Rol, Rol>().ReverseMap();


        CreateMap<Rol, RolxPersonDto>().ReverseMap();
      }
    }