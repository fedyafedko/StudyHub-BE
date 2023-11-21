﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudyHub.BLL.Services.Interfaces;
using StudyHub.Common.DTO;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Models;
using StudyHub.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudyHub.BLL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtSettings _settings;

    public AuthService(UserManager<User> userManager, IOptions<JwtSettings> settings)
    {
        _userManager = userManager;
        _settings = settings.Value;
    }

    public async Task<AuthSuccessDTO> LoginAsync(LoginUserDTO user)
    {
        var existingUser = await _userManager.FindByEmailAsync(user.Email);

        if (existingUser == null)
            throw new NotFoundException($"Unable to find user by specified email. Email: {user.Email}");

        var result = await _userManager.CheckPasswordAsync(existingUser, user.Password);

        if(!result)
            throw new InvalidCredentialsException($"User input incorrect password. Password: {user.Password}");

        return new AuthSuccessDTO(GenerateJwtToken(existingUser));
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
                new Claim("email", user.Email!),
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
