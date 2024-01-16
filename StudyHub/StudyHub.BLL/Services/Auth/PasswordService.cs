using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StudyHub.BLL.Services.Interfaces.Auth;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Models;
using StudyHub.Common.Requests;
using StudyHub.Entities;
using StudyHub.FluentEmail.MessageBase;
using StudyHub.FluentEmail.Services.Interfaces;

namespace StudyHub.BLL.Services.Auth;

public class PasswordService : IPasswordService
{
    private readonly UserManager<User> _userManager;
    private readonly EmailSettings _messageSettings;
    private readonly IEmailService _emailService;

    public PasswordService(
        UserManager<User> userManager,
        IOptions<EmailSettings> messageSettings,
        IEmailService emailService)
    {
        _userManager = userManager;
        _messageSettings = messageSettings.Value;
        _emailService = emailService;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new NotFoundException($"User with this email does not exist. Email: {dto.Email}");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var uri = string.Format(_messageSettings.AcceptInvitationUrl, user.Email, token);

        var emailSent = await _emailService.SendAsync(user.Email!,
            new ResetPasswordMessage { Email = user.Email!, ResetPasswordUri = uri });

        return emailSent;
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