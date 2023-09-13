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
        return await _context.Users
        .Include(u => u.Rols)
        .Include(u => u.RefreshTokens)
        .FirstOrDefaultAsync (u => u.NameUser.ToLower()==userName.ToLower());
    }

      public async Task<User> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Users
            .Include(u => u.Rols)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
    }
}