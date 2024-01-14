using StudyHub.Common.DTO.AuthDTO;

namespace StudyHub.BLL.Services.Interfaces.Auth;
public interface IPasswordService
{
    Task<string> ForgotPassword(ForgotPasswordDTO dto);
    Task<bool> IsResetPassword(ResetPasswordDTO dto);
}
