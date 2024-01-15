using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.Exceptions;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;
using System.IdentityModel.Tokens.Jwt;
using StudyHub.BLL.Services.Interfaces.Auth;
using StudyHub.Common.Requests;

namespace StudyHub.BLL.Services.Auth;

public class AuthService : IAuthService
{
    protected readonly UserManager<User> _userManager;
    protected readonly ITokenService _tokenService;
    private readonly IRepository<InvitedUser> _invitedUserRepository;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;
    protected readonly IMapper _mapper;

    public AuthService(
        UserManager<User> userManager,
        ITokenService tokenService,
        IRepository<InvitedUser> invitedUserRepository,
        IMapper mapper,
        IRepository<RefreshToken> refreshTokenRepository)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _invitedUserRepository = invitedUserRepository;
        _mapper = mapper;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthSuccessDTO> LoginAsync(LoginUserDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new NotFoundException($"Unable to find user by specified email. Email: {dto.Email}");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isPasswordValid)
            throw new InvalidCredentialsException($"User input incorrect password. Password: {dto.Password}");

        return await GetAuthTokensAsync(user);
    }

    public async Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO dto)
    {
        var invitedUser = _invitedUserRepository.FirstOrDefault(e => e.Email == dto.Email)
            ?? throw new NotFoundException($"User with this email wasn't invited: {dto.Email}");

        if (invitedUser.Token != dto.Token)
            throw new IncorrectParametersException("Passed token isn't valid.");

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user != null)
            throw new AlreadyExistsException($"User with specified email already exists. Email: {dto.Email}");

        user = _mapper.Map<User>(dto);

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            throw new UserManagerException($"User manager operation failed:\n", result.Errors);

        var role = invitedUser.Role;

        var currentUser = await _userManager.FindByIdAsync(user.Id.ToString());

        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
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

        var existingToken = _refreshTokenRepository.FirstOrDefault(rf => rf.Token == request.RefreshToken) 
            ?? throw new NotFoundException($"This token doesn't found: {request.RefreshToken}");

        if (DateTimeOffset.UtcNow > existingToken.ExpiryDate)
            throw new ExpiredException("Refresh token is expired");

        if (existingToken.Invalidated)
            throw new TokenValidatorException($"This token is invalidated: {request.RefreshToken}");
        
        await _refreshTokenRepository.DeleteAsync(existingToken);

        var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value)
            ?? throw new NotFoundException("User with this id does not exist");

        return await GetAuthTokensAsync(user);
    }

    protected async Task<AuthSuccessDTO> GetAuthTokensAsync(User user)
    {
        return new AuthSuccessDTO(await _tokenService.GenerateJwtTokenAsync(user!),
           await _tokenService.GenerateRefreshTokenAsync(user!));
    }
}
