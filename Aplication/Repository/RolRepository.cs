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

    public override async Task<(int totalRegistros, IEnumerable<Rol> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Rols as IQueryable<Rol>;
        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.NameRol.ToLower().Contains(search));
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Include(p => p.Users)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (totalRegistros, registros);
    }

    public override async Task<IEnumerable<Rol>> GetAllAsync()
    {
        return await _context.Rols.Include(p => p.Users)
        .ToListAsync();
    }

}