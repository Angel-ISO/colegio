using Dominio;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;
public class RolRepository : GenericRepository<Rol>, IRol
{
        private readonly ColegioContext _context;

    public RolRepository(ColegioContext context) : base(context)
    {
           _context = context;     
    }

      public override async Task<IEnumerable<Rol>> GetAllAsync()
    {
        return await _context.Rols.Include(p => p.Users)
        .ToListAsync();
    }
}