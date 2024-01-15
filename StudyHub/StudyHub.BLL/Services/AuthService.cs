using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.Exceptions;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Tokens.Jwt;

namespace StudyHub.BLL.Services;

// ToDo: Implement Refresh Token
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IRepository<InvitedUser> _invitedUserRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;

    public AuthService(
        UserManager<User> userManager,
        ITokenService tokenService,
        IRepository<InvitedUser> invitedUserRepository,
        IMapper mapper,
        IRepository<RefreshToken> refreshTokenRepository)
    {
        _mapper = mapper;
        _invitedUserRepository = invitedUserRepository;
        _userManager = userManager;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthSuccessDTO> LoginAsync(LoginUserDTO user)
    {
        var existingUser = await _userManager.FindByEmailAsync(user.Email)
            ?? throw new NotFoundException($"Unable to find user by specified email. Email: {user.Email}");

        var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, user.Password);

        if (!isPasswordValid)
            throw new InvalidCredentialsException($"User input incorrect password. Password: {user.Password}");

        return await GetAuthTokens(existingUser);
    }

    public async Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO dto)
    {
        var invitedUser = _invitedUserRepository.FirstOrDefault(e => e.Email == dto.Email)
            ?? throw new NotFoundException($"User with this email wasn't invited: {dto.Email}");

        if (invitedUser.Token != dto.Token)
            throw new IncorrectParametersException("Passed token isn't valid.");

        var user = _mapper.Map<User>(dto);

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            throw new UserManagerException($"User manager operation failed:\n", result.Errors);

        var role = invitedUser.Role;

        var currentUser = await _userManager.FindByIdAsync(user.Id.ToString());

        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
            throw new UserManagerException($"User manager operation failed:\n", result.Errors);

        return await GetAuthTokens(user);
    }

    public async Task<AuthSuccessDTO> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var validatedToken = _tokenService.GetPrincipalFromToken(accessToken);

        var expiryDateUnix = long.Parse(validatedToken!.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        var expiryDateTimeUtc = new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow)
            throw new IncorrectParametersException("Access token is not expired yet");

        var jti = validatedToken!.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;

        var existingToken = _refreshTokenRepository.FirstOrDefault(rf => rf.Token == refreshToken) 
            ?? throw new NotFoundException($"This token doesn't found: {refreshToken}");

        if (DateTimeOffset.UtcNow > existingToken.ExpiryDate)
            throw new ExpiredException("Refresh token is expired");

        if (existingToken.Invalidated)
            throw new TokenValidatorException($"This token is invaludated: {refreshToken}");
        
        await _refreshTokenRepository.DeleteAsync(existingToken);

        var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value)
            ?? throw new NotFoundException("User with this id does not exist");

        return await GetAuthTokens(user);
    }

    private async Task<AuthSuccessDTO> GetAuthTokens(User user)
    {
        var roles = (await _userManager.GetRolesAsync(user)).ToArray();
        return new AuthSuccessDTO(_tokenService.GenerateJwtToken(user!, roles),
            await _tokenService.GenerateRefreshTokenAsync(user!));
    }
}
