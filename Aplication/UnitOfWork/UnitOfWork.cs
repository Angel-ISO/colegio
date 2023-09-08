using Dominio.Interfaces;
using Persistencia;
using Aplicacion.Repository;

namespace Aplicacion.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ColegioContext _context;

    private UserRepository _Usuario;
    private RolRepository _Rol;
    private InscriptionRepository _Inscription;  
    private CursoRepository _Curso;
    private PersonRepository _Person;  
    public UnitOfWork(ColegioContext context)
    {
        _context = context;
    }

    public IUser Users
    {
        get
        {
            if (_Usuario is not null)
            {
                return _Usuario;
            }
            return _Usuario = new UserRepository(_context);
        }
    }

     public IPerson Persons
    {
        get
        {
            if (_Person is not null)
            {
                return _Person;
            }
            return _Person = new PersonRepository(_context);
        }
    }




     public ICurso Courses
    {
        get
        {
            if (_Curso is not null)
            {
                return _Curso;
            }
            return _Curso = new CursoRepository(_context);
        }
    }


     public IInscription Inscriptions
    {
        get
        {
            if (_Inscription is not null)
            {
                return _Inscription;
            }
            return _Inscription = new InscriptionRepository(_context);
        }
    }


    public IRol Rols
    {
        get
        {
            if (_Rol is not null)
            {
                return _Rol;
            }
            return _Rol = new RolRepository(_context);
        }
    }


    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

}