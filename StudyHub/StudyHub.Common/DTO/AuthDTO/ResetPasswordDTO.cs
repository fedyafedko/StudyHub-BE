﻿namespace StudyHub.Common.DTO.AuthDTO;

public class ResetPasswordDTO
{
    public string ResetToken { get; set; } = string.Empty;
    public string Email {  get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfimPassword { get; set; } = string.Empty;
}
