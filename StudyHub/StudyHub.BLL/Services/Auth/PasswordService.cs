using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StudyHub.BLL.Services.Interfaces.Auth;
using StudyHub.Common.Configs;
using StudyHub.Common.Exceptions;
using StudyHub.Common.Requests;
using StudyHub.Entities;
using StudyHub.FluentEmail.MessageBase;
using StudyHub.FluentEmail.Services.Interfaces;

namespace StudyHub.BLL.Services.Auth;

public class PasswordService : IPasswordService
{
    private readonly UserManager<User> _userManager;
    private readonly EmailConfig _emailConfig;
    private readonly IEmailService _emailService;

    public PasswordService(
        UserManager<User> userManager,
        IOptions<EmailConfig> emailConfig,
        IEmailService emailService)
    {
        _userManager = userManager;
        _emailConfig = emailConfig.Value;
        _emailService = emailService;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException($"User with this email does not exist. Email: {request.Email}");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var uri = string.Format(_emailConfig.AcceptInvitationUrl, user.Email, token);

        var emailSent = await _emailService.SendAsync(request.Email,
            new ResetPasswordMessage { Recipient = request.Email, ResetPasswordUri = uri });

        return emailSent;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException($"Unable to find user by specified email. Email: {request.Email}");

        var isSamePassword = await _userManager.CheckPasswordAsync(user, request.NewPassword);

        if (isSamePassword)
            throw new IncorrectParametersException("New password have to differ from the old one");

        var result = await _userManager.ResetPasswordAsync(user, request.ResetToken, request.NewPassword);

        if (!result.Succeeded)
            throw new UserManagerException("Unable to reset password", result.Errors);

        return result.Succeeded;
    }
}