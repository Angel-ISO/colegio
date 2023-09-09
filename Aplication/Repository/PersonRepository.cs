using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;
public class PersonRepository : GenericRepository<Person>, IPerson
{
        private readonly ColegioContext _context;
    public PersonRepository(ColegioContext context) : base(context)
    {
            _context = context;
    }

     public override async Task<(int totalRegistros, IEnumerable<Person> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Persons as IQueryable<Person>;
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.ToLower().Contains(search));
        }
        var totalRegistros = await query.CountAsync();
        var registros = await query
                                 .Include(u => u.Cursos)
                                 .Include(p => p.Inscriptions)
                                 .Skip((pageIndex - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        return (totalRegistros, registros);
    }

    public override async Task<IEnumerable<Person>> GetAllAsync()
{
    return await _context.Persons
        .Include(p => p.Cursos).
        Include(o => o.Inscriptions)
       // .ThenInclude(d => d.Inscriptions) // Herencia multiple (no se como hacerlo aun)
        .ToListAsync();
}



}