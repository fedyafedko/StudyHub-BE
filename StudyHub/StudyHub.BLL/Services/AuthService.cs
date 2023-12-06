using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Models;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StudyHub.BLL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtSettings _settings;
    private readonly TokenValidationParameters _tokenValidationParametrs;
    private readonly IRepository<InvitedUsers> _repoInvitedUsers;

    public AuthService(
        UserManager<User> userManager,
        IOptions<JwtSettings> settings,
        TokenValidationParameters tokenValidationParameters,
        IRepository<InvitedUsers> repoInvitedUsers)
    {
        _repoInvitedUsers = repoInvitedUsers;
        _userManager = userManager;
        _settings = settings.Value;
        _tokenValidationParametrs = tokenValidationParameters;
    }

    public async Task<AuthSuccessDTO> LoginAsync(LoginUserDTO user)
    {
        var existingUser = await _userManager.FindByEmailAsync(user.Email);

        if (existingUser == null)
            throw new NotFoundException($"Unable to find user by specified email. Email: {user.Email}");

        var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, user.Password);

        if (!isPasswordValid)
            throw new InvalidCredentialsException($"User input incorrect password. Password: {user.Password}");

        return new AuthSuccessDTO(GenerateJwtToken(existingUser, (await _userManager.GetRolesAsync(existingUser)).ToArray()), GenerateRefreshTokenAsync(existingUser));
    }

    public async Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO user)
    {
        var userInvited = _repoInvitedUsers.FirstOrDefault(a => a.Token == user.Token);
        if (userInvited == null)
            throw new NotFoundException($"You doesn't invited by this token:{user.Token}");

        if(userInvited.Email != user.Email)
            throw new IncorrectParametersException("Not correct email");

        var existingUser = await _userManager.FindByEmailAsync(user.Email);

        if (existingUser != null)
            throw new NotFoundException($"User with specified email already exists. Email: {user.Email}");

        var newUser = new User()
        {
            Email = user.Email,
            UserName = user.Email,
        };

        var result = await _userManager.CreateAsync(newUser, user.Password);

        if (!result.Succeeded)
            throw new UserManagerException($"User manager operation failed:\n", result.Errors);

        if (userInvited.Role == "Teacher")
        {
            var newTeacher = new Teacher()
            {
                User = newUser,
                UserId = newUser.Id,
            };
            var roleresult = await _userManager.AddToRoleAsync(newUser, "Teacher");
            if (!roleresult.Succeeded)
                throw new UserManagerException($"User manager operation failed:\n", result.Errors);
        }

        if (userInvited.Role == "Student")
        {
            var newStudent = new Student()
            {
                User = newUser,
                UserId = newUser.Id,
            };
            await _userManager.AddToRoleAsync(newUser, "Student");
        }

        return new AuthSuccessDTO(GenerateJwtToken(newUser , (await _userManager.GetRolesAsync(newUser)).ToArray()), GenerateRefreshTokenAsync(newUser));
    }

    public async Task<AuthSuccessDTO> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(accessToken);

        var expiryDateUnix = long.Parse(validatedToken!.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        var expiryDateTimeUtc = new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow)
            throw new IncorrectParametersException("Access token is not expired yet");

        var jti = validatedToken!.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;

        var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);

        if (user is null)
            throw new NotFoundException("User with this id does not exist");

        if (DateTimeOffset.UtcNow > user.RefreshToken.ExpiryDate)
            throw new ExpiredException("Refresh token is expired");

        if (user.RefreshToken.Token != refreshToken)
            throw new IncorrectParametersException("Refresh token is invalid");

        return new AuthSuccessDTO(GenerateJwtToken(user!, (await _userManager.GetRolesAsync(user)).ToArray()), GenerateRefreshTokenAsync(user!));
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var validationParametrs = _tokenValidationParametrs.Clone();
        validationParametrs.ValidateLifetime = false;
        try
        {
            var principal = jwtTokenHandler.ValidateToken(token, validationParametrs, out var validatedToken);

            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                throw new InvalidSecurityAlgorithmException("Current token does not have right security algorithm");

            return principal;
        }
        catch
        {
            throw new TokenValidatorException("Something went wrong with token validator");
        }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return validatedToken is JwtSecurityToken jwtSecurityToken &&
            jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
    }

    private string GenerateJwtToken(User user, string[] roles)
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

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

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
    private string GenerateRefreshTokenAsync(User user)
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

        user.RefreshToken = refreshToken;

        return user.RefreshToken.Token;
    }
}
