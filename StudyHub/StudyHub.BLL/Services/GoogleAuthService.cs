using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO.AuthDTO;
using StudyHub.Common.Exceptions;
using StudyHub.Entities;
using Microsoft.AspNetCore.Identity;
using StudyHub.Common.Models;
using Microsoft.Extensions.Options;

namespace StudyHub.BLL.Services;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenManagementService _jwtTokenManagementService;
    private readonly GoogleAuthConfig _googleConfig;

    public GoogleAuthService(
        UserManager<User> userManager,
        IJwtTokenManagementService jwtTokenManagementService,
        IOptions<GoogleAuthConfig> googleConfig)
    {
        _userManager = userManager;
        _jwtTokenManagementService = jwtTokenManagementService;
        _googleConfig = googleConfig.Value;
    }
    public async Task<AuthSuccessDTO> GoogleLogin(string oauthToken)
    {
        var paylod = await GetUserInfoAsync(oauthToken);

        var user = await _userManager.FindByEmailAsync(paylod.Email);

        if (user == null)
            throw new NotFoundException($"Unable to find user by specified email. Email: {user!.Email}");

        return new AuthSuccessDTO(
            _jwtTokenManagementService.GenerateJwtToken(user), 
            _jwtTokenManagementService.GenerateRefreshTokenAsync(user));
    }

    private async Task<GoogleJsonWebSignature.Payload> GetUserInfoAsync(string oauthToken)
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
            oauthToken,
            _googleConfig.RedirectUri,
            CancellationToken.None);

        var setting = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { _googleConfig.ClientId },
            IssuedAtClockTolerance = _googleConfig.IssuedAtClockTolerance,
            ExpirationTimeClockTolerance = _googleConfig.ExpirationTimeClockTolerance,
        };

        var paylod = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken, setting);
        return paylod;
    }
}
