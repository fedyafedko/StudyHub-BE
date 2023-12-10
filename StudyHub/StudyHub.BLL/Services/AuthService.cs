using Microsoft.IdentityModel.Tokens;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.Exceptions;
using StudyHub.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudyHub.BLL.Services;

public class AuthService : IAuthService
{
    private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
    private readonly IJwtTokenManagementService _jwtTokenManagementService;
    private readonly TokenValidationParameters _tokenValidationParametrs;
    public AuthService(
        Microsoft.AspNetCore.Identity.UserManager<User> userManager,
        IJwtTokenManagementService jwtTokenManagementService,
        TokenValidationParameters tokenValidationParameters)
    {
        _userManager = userManager;
        _jwtTokenManagementService = jwtTokenManagementService;
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

        return new AuthSuccessDTO(
            _jwtTokenManagementService.GenerateJwtToken(existingUser), 
            _jwtTokenManagementService.GenerateRefreshTokenAsync(existingUser));
    }

    public async Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO user)
    {
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

        return new AuthSuccessDTO(
            _jwtTokenManagementService.GenerateJwtToken(newUser), 
            _jwtTokenManagementService.GenerateRefreshTokenAsync(newUser));
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

        return new AuthSuccessDTO(
            _jwtTokenManagementService.GenerateJwtToken(user!), 
            _jwtTokenManagementService.GenerateRefreshTokenAsync(user!));
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
}
