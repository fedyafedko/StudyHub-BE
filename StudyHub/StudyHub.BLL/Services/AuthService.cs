using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Plugins;
using StudyHub.BLL.Services.Interface;
using StudyHub.Common.DTO;
using StudyHub.Common.Models;
using StudyHub.DAL.Repositories;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudyHub.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly JwtSettings _settings;
    public async Task<AuthSuccessDTO> LoginAsync(LoginUserDTO user)
    {
        string hashedPassword = _passwordHasher.Hash(user.Password);
        var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == user.Email);

        if (existingUser == null)
            throw new KeyNotFoundException(user.Email);

        if (existingUser.PasswordHash != hashedPassword)
            throw new UnauthorizedAccessException(user.Email);

        return new AuthSuccessDTO(GenerateJwtToken(existingUser));

    }

    public async Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO user)
    {
        string hashedPassword = _passwordHasher.Hash(user.Password);
        var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == user.Email);

        if (existingUser != null)
            throw new InvalidOperationException(user.Email);

        var newUser = new User()
        {
            Email = user.Email,
            PasswordHash = hashedPassword,
        };
        _userRepository.Insert(newUser);

        return new AuthSuccessDTO(GenerateJwtToken(newUser));
    }

    private string GenerateJwtToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        return jwtToken;
    }
}
