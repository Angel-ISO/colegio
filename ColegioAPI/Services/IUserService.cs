using ColegioAPI.Dtos;

namespace ColegioAPI.Services;

    public interface IUserService
    {
      Task <string> RegisterAsync (RegisterDto model);
      Task <DatosUsuarioDto> GetTokenAsync (LoginDto model);
      Task <string> AddRoleAsync (AddRoleDto model);
      Task<DatosUsuarioDto> RefreshTokenAsync(string refreshToken);
    }