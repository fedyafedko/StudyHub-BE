using Microsoft.AspNetCore.Identity;
using StudyHub.BLL.Services.Interfaces.Auth;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Requests;
using StudyHub.Entities;

namespace StudyHub.BLL.Services.Auth;

public class PasswordService : IPasswordService
{
    private readonly UserManager<User> _userManager;

    public PasswordService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new NotFoundException($"User with this email does not exist. Email: {dto.Email}");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return token;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new NotFoundException($"Unable to find user by specified email. Email: {dto.Email}");

        var isSamePassword = await _userManager.CheckPasswordAsync(user, dto.NewPassword);

        if (isSamePassword)
            throw new IncorrectParametersException("New password have to differ from the old one");

        var result = await _userManager.ResetPasswordAsync(user, dto.ResetToken, dto.NewPassword);

        if (!result.Succeeded)
            throw new UserManagerException("Unable to reset password", result.Errors);

        return result.Succeeded;
    }
}
