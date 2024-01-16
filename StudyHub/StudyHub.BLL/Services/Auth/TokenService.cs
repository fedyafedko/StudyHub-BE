using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudyHub.BLL.Extensions;
using StudyHub.BLL.Services.Interfaces.Auth;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Models;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StudyHub.BLL.Services.Auth;

public class TokenService : ITokenService
{
    private readonly JwtSettings _settings;
    private readonly UserManager<User> _userManager;
    private readonly TokenValidationParameters _tokenValidationParametrs;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;

    public TokenService(IOptions<JwtSettings> settings, 
        TokenValidationParameters tokenValidationParametrs,
        IRepository<RefreshToken> refreshTokenRepository, 
        UserManager<User> userManager)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _settings = settings.Value;
        _userManager = userManager;
        _tokenValidationParametrs = tokenValidationParametrs;
    }

    public async Task<string> GenerateJwtTokenAsync(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_settings.Secret);

        var claims = new List<Claim>
        {
            new Claim("id", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_settings.AccessTokenLifeTime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        return jwtToken;
    }

    public async Task<string> GenerateRefreshTokenAsync(User user)
    {
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.Now.Add(_settings.RefreshTokenLifeTime),
            Used = true,
            Invalidated = false,
        };

        await _refreshTokenRepository.InsertAsync(refreshToken);

        user.RefreshTokenId = refreshToken.Id;

        await _userManager.UpdateAsync(user);

        return refreshToken.Token;
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var validationParametrs = _tokenValidationParametrs.Clone();
        validationParametrs.ValidateLifetime = false;
        try
        {
            var principal = jwtTokenHandler.ValidateToken(token, validationParametrs, out var validatedToken);

            if (!validatedToken.IsJwtWithValidSecurityAlgorithm())
                throw new InvalidSecurityAlgorithmException("Current token does not have right security algorithm");

            return principal;
        }
        catch
        {
            throw new TokenValidatorException("Something went wrong with token validator");
        }
    }
}