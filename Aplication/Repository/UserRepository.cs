using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;


namespace Aplicacion.Repository;
public class UserRepository : GenericRepository<User>, IUser
{
    private readonly ColegioContext _context;
    public UserRepository(ColegioContext context) : base(context)
    {
        _context = context;
    }
    public async Task<User> GetByUserNameAsync (string userName)
    {
        return await _context.Users.Include(u => u.Rols).FirstOrDefaultAsync (u => u.NameUser.ToLower()==userName.ToLower());
    }
}