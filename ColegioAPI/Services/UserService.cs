using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ColegioAPI.Dtos;
using ColegioAPI.Helpers;
using Dominio;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ColegioAPI.Services;
public class UserService : IUserService
{
    private readonly JWT _jwt;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<User> passwordHasher)
    {
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

         public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var usuario = new User
            {
                Email = registerDto.Email,
                Username = registerDto.Username,

            };

            usuario.Password = _passwordHasher.HashPassword(usuario, registerDto.Password);

            var usuarioExiste = _unitOfWork.Users
                                                .Find(u => u.Username.ToLower() == registerDto.Username.ToLower())
                                                .FirstOrDefault();

            if (usuarioExiste == null)
            {

                try
                {
                    //usuario.Rols.Add(rolPredeterminado);
                    _unitOfWork.Users.Add(usuario);
                    await _unitOfWork.SaveAsync();

                    return $"El Usuario {registerDto.Username} ha sido registrado exitosamente";
                }

                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }
            }
            else
            {

                return $"El usuario con {registerDto.Username} ya se encuentra resgistrado.";
            }

        }
     

 

    public async Task<string> AddRoleAsync(AddRoleDto model)
    {
        var usuario = await _unitOfWork.Users
                                                .GetByUserNameAsync(model.Username);

        if (usuario == null)
        {
            return $"No existe algun usuario registrado con la cuenta olvido algun caracter?{model.Username}.";
        }

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

        if (resultado == PasswordVerificationResult.Success)
        {
            var rolExiste = _unitOfWork.Rols
                                            .Find(u => u.NameRol.ToLower() == model.Role.ToLower())
                                            .FirstOrDefault();

            if (rolExiste != null)
            {
                var usuarioTieneRol = usuario.Rols
                                                .Any(u => u.Id == rolExiste.Id);

                if (usuarioTieneRol == false)
                {
                    usuario.Rols.Add(rolExiste);
                    _unitOfWork.Users.Update(usuario);
                    await _unitOfWork.SaveAsync();
                }

                return $"Rol {model.Role} agregado a la cuenta {model.Username} de forma exitosa.";
            }

            return $"Rol {model.Role} no encontrado.";
        }

        return $"Credenciales incorrectas para el ususario {usuario.Username}.";
    }




   /*  public async Task<DatosUsuarioDto> GetTokenAsync(LoginDto model)
    {
        DatosUsuarioDto datosUsuarioDto = new DatosUsuarioDto();
        var usuario = await _unitOfWork.Users
                                                    .GetByUserNameAsync(model.Username);

        if (usuario == null)
        {
            datosUsuarioDto.EstaAutenticado = false;
            datosUsuarioDto.Mensaje = $"No existe ningun usuario con el username {model.Username}.";
            return datosUsuarioDto;
        }

        var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);
        if (result == PasswordVerificationResult.Success)
        {
            datosUsuarioDto.Mensaje = "OK";
            datosUsuarioDto.EstaAutenticado = true;
            if (usuario != null && usuario != null)
            {
                JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
                datosUsuarioDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                datosUsuarioDto.Username = usuario.Username;
                datosUsuarioDto.Email = usuario.Email;
                datosUsuarioDto.Rols = usuario.Rols
                                                    .Select(p => p.NameRol)
                                                    .ToList();


                return datosUsuarioDto;
            }
            else
            {
                datosUsuarioDto.EstaAutenticado = false;
                datosUsuarioDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.Username}.";

                return datosUsuarioDto;
            }
        }

        datosUsuarioDto.EstaAutenticado = false;
        datosUsuarioDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.Username}.";

        return datosUsuarioDto;

    } */



 public async Task<DatosUsuarioDto> GetTokenAsync(LoginDto model)
    {
        DatosUsuarioDto userDataDto = new DatosUsuarioDto();
        var user = await _unitOfWork.Users
                    .GetByUserNameAsync(model.Username);

        if (user == null)
        {
            userDataDto.EstaAutenticado = false;
            userDataDto.Mensaje = $"User does not exists {model.Username}.";
            return userDataDto;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

        if (result == PasswordVerificationResult.Success)
        {
            userDataDto.Mensaje = "OK";
            userDataDto.EstaAutenticado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(user);
            userDataDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            userDataDto.Email = user.Email;
            userDataDto.UserName = user.Username;
            userDataDto.Rols = user.Rols
                                            .Select(u => u.NameRol)
                                            .ToList();

            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                userDataDto.RefreshToken = activeRefreshToken.Token;
                userDataDto.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                userDataDto.RefreshToken = refreshToken.Token;
                userDataDto.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveAsync();
            }

            return userDataDto;
        }
        userDataDto.EstaAutenticado = false;
        userDataDto.Mensaje = $"Invalid Credentials {user.Username}.";
        return userDataDto;
    }
  
/* 
    public async Task<DatosUsuarioDto> RefreshTokenAsync(string refreshToken)
    {
        var dataUserDto = new DatosUsuarioDto();

        var usuario = await _unitOfWork.Users
                        .GetByRefreshTokenAsync(refreshToken);
        Console.WriteLine(refreshToken);
        Console.WriteLine();

        if (usuario == null)
        {
            dataUserDto.EstaAutenticado = false;
            dataUserDto.Mensaje = $"el token no existe. Porfavor generar uno nuevo";
            return dataUserDto;
        }

        var refreshTokenBd = usuario.RefreshTokens.Single(x => x.Token == refreshToken);

        if (!refreshTokenBd.IsActive)
        {
            dataUserDto.EstaAutenticado = false;
            dataUserDto.Mensaje = $"Token inactivo.";
            return dataUserDto;
        }
        //intercambiar la fecha por la actual
        refreshTokenBd.Revoked = DateTime.UtcNow;
        //aqui se crea el nuevo jwt y se guarda en la base de datos
        var newRefreshToken = CreateRefreshToken();
        usuario.RefreshTokens.Add(newRefreshToken);
        _unitOfWork.Users.Update(usuario);
        await _unitOfWork.SaveAsync();
        //aqui se genera 
        dataUserDto.EstaAutenticado = true;
        JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
        dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        dataUserDto.Email = usuario.Email;
        dataUserDto.UserName = usuario.Username;
        dataUserDto.Rols = usuario.Rols
                                        .Select(u => u.NameRol)
                                        .ToList();
        dataUserDto.RefreshToken = newRefreshToken.Token;
        dataUserDto.RefreshTokenExpiration = newRefreshToken.Expires;
        return dataUserDto;
    }*/


    private RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddDays(10),
                Created = DateTime.UtcNow
            };
        }
    }
 



public async Task<DatosUsuarioDto> RefreshTokenAsync(string refreshToken)
{
    var dataUserDto = new DatosUsuarioDto();

    try
    {
        var usuario = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken);

        if (usuario == null)
        {
            return GenerateTokenNotFoundResponse(dataUserDto);
        }

        var refreshTokenBd = usuario.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

        if (refreshTokenBd == null || !refreshTokenBd.IsActive)
        {
            return GenerateInactiveTokenResponse(dataUserDto);
        }

        refreshTokenBd.Revoked = DateTime.UtcNow;

        var newRefreshToken = CreateRefreshToken();

        usuario.RefreshTokens.Add(newRefreshToken);

        _unitOfWork.Users.Update(usuario);
        await _unitOfWork.SaveAsync();

        var jwtSecurityToken = CreateJwtToken(usuario);

        PopulateDataUserDto(dataUserDto, usuario, jwtSecurityToken, newRefreshToken);

        return dataUserDto;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return GenerateErrorResponse(dataUserDto);
    }
}

private DatosUsuarioDto GenerateTokenNotFoundResponse(DatosUsuarioDto dataUserDto)
{
    dataUserDto.EstaAutenticado = false;
    dataUserDto.Mensaje = "El token no existe. Por favor, genere uno nuevo.";
    return dataUserDto;
}


private DatosUsuarioDto GenerateInactiveTokenResponse(DatosUsuarioDto dataUserDto)
{
    dataUserDto.EstaAutenticado = false;
    dataUserDto.Mensaje = "Token inactivo.";
    return dataUserDto;
}

private DatosUsuarioDto GenerateErrorResponse(DatosUsuarioDto dataUserDto)
{
    // Lógica para generar una respuesta de error aquí.
    // ...
    return dataUserDto;
}

private void PopulateDataUserDto(DatosUsuarioDto dataUserDto, User usuario, JwtSecurityToken jwtToken, RefreshToken refreshToken)
{
    dataUserDto.EstaAutenticado = true;
    dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
    dataUserDto.Email = usuario.Email;
    dataUserDto.UserName = usuario.Username;
    dataUserDto.Rols = usuario.Rols.Select(u => u.NameRol).ToList();
    dataUserDto.RefreshToken = refreshToken.Token;
    dataUserDto.RefreshTokenExpiration = refreshToken.Expires;
}














    private JwtSecurityToken CreateJwtToken(User usuario)
    {
        if (usuario == null)
        {
            throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
        }

        var roles = usuario.Rols;
        var roleClaims = new List<Claim>();
        foreach (var role in roles)
        {
            roleClaims.Add(new Claim("roles", role.NameRol));
        }

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("uid", usuario.Id.ToString())
    }
        .Union(roleClaims);

        if (string.IsNullOrEmpty(_jwt.Key) || string.IsNullOrEmpty(_jwt.Issuer) || string.IsNullOrEmpty(_jwt.Audience))
        {
            throw new ArgumentNullException("La configuración del JWT es nula o vacía.");
        }

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

        var JwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);

        return JwtSecurityToken;
    }


    /*public async Task<LoginDto>  UserLogin(LoginDto model)
    {
        var usuario = await _unitOfWork.Usuarios.GetByUsernameAsync(model.Username);
        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

        if (resultado == PasswordVerificationResult.Success)
        {
            return model;
        }
        return null;
    }*/

}