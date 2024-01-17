using AutoMapper;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StudyHub.BLL.Services.Interfaces.Auth;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Models;
using StudyHub.DAL.Repositories.Interfaces;
using StudyHub.Entities;

namespace StudyHub.BLL.Services.Auth;

public class GoogleAuthService : AuthService, IGoogleAuthService
{
    private readonly GoogleAuthConfig _googleConfig;

    public GoogleAuthService(
        IRepository<InvitedUser> invitedUserRepository,
        IRepository<RefreshToken> refreshTokenRepository,
        ITokenService tokenService,
        IEncryptService encryptService,
        IOptions<GoogleAuthConfig> googleConfig,
        IMapper mapper)
            : base(userManager, tokenService, encryptService, invitedUserRepository, refreshTokenRepository, mapper)
    {
        _googleConfig = googleConfig.Value;
    }

    public async Task<AuthSuccessDTO> GoogleRegisterAsync(string authorizationCode)
    {
        var payload = await GetGooglePayloadAsync(authorizationCode);

        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user != null)
            throw new NotFoundException("User with this email already exists");

        user = _mapper.Map<User>(payload);

        var createdUserResult = await _userManager.CreateAsync(user);

        if (!createdUserResult.Succeeded)
            throw new UserManagerException("Unable to authenticate given user", createdUserResult.Errors);

        return await GetAuthTokensAsync(user);
    }

    public async Task<AuthSuccessDTO> GoogleLoginAsync(string authorizationCode)
    {
        var paylod = await GetGooglePayloadAsync(authorizationCode);

        var user = await _userManager.FindByEmailAsync(paylod.Email);

        if (user == null)
            throw new NotFoundException($"Unable to find user by specified email. Email: {user!.Email}");

        return await GetAuthTokensAsync(user);
    }

    private async Task<GoogleJsonWebSignature.Payload> GetGooglePayloadAsync(string authorizationCode)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleConfig.ClientId,
                ClientSecret = _googleConfig.ClientSecret
            }
        });

        var tokenResponse = await flow.ExchangeCodeForTokenAsync(
            string.Empty,
            authorizationCode,
            _googleConfig.RedirectUri,
            CancellationToken.None);

        var setting = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { _googleConfig.ClientId },
            IssuedAtClockTolerance = _googleConfig.IssuedAtClockTolerance,
            ExpirationTimeClockTolerance = _googleConfig.ExpirationTimeClockTolerance,
        };

        return await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken, setting);
    }
}