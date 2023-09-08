namespace Dominio.Interfaces;
public interface IUnitOfWork
{
    IUser Users {get;}   
    IRol Rols {get;}   
    IInscription Inscriptions {get;}   
    ICurso Courses {get;}   
    IPerson Persons {get;}   
  
    Task<int> SaveAsync();
}