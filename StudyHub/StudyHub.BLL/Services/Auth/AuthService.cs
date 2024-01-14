using Microsoft.IdentityModel.Tokens;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.Exceptions;
using StudyHub.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using StudyHub.BLL.Services.Interfaces.Auth;
using AutoMapper;

namespace StudyHub.BLL.Services.Auth;

// ToDo: Implement Refresh Token
public class AuthService : IAuthService
{
    protected readonly UserManager<User> _userManager;
    protected readonly ITokenService _tokenService;
    protected readonly IMapper _mapper;

    public AuthService(
        UserManager<User> userManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AuthSuccessDTO> LoginAsync(LoginUserDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            throw new NotFoundException($"Unable to find user by specified email. Email: {dto.Email}");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isPasswordValid)
            throw new InvalidCredentialsException($"User input incorrect password. Password: {dto.Password}");

        return await GetAuthTokensAsync(user);
    }

    public async Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user != null)
            throw new NotFoundException($"User with specified email already exists. Email: {dto.Email}");

        user = _mapper.Map<User>(dto);

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            throw new UserManagerException($"User manager operation failed:\n", result.Errors);

        return await GetAuthTokensAsync(user);
    }

    public async Task<AuthSuccessDTO> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var validatedToken = _tokenService.GetPrincipalFromToken(request.AccessToken);

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

        if (user.RefreshToken.Token != request.RefreshToken)
            throw new IncorrectParametersException("Refresh token is invalid");

        return await GetAuthTokensAsync(user);
    }

    protected async Task<AuthSuccessDTO> GetAuthTokensAsync(User user)
    {
        return new AuthSuccessDTO(await _tokenService.GenerateJwtTokenAsync(user!),
            _tokenService.GenerateRefreshTokenAsync(user!));
    }
}
