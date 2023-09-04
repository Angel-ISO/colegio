namespace Dominio.Interfaces;
public interface IUnitOfWork
{
    IUser Users {get;}   
    IRol Roles {get;}   
    IInscription Inscriptions {get;}   
    ICurso Courses {get;}   

    
      
    Task<int> SaveAsync();
}