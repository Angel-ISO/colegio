using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;
public class CursoRepository : GenericRepository<Curso>, ICurso
{
    public CursoRepository(ColegioContext context) : base(context)
    {
    }
}