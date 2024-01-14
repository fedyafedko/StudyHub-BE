﻿using StudyHub.Common.DTO.AuthDTO;

namespace StudyHub.BLL.Services.Interfaces.Auth;

public interface IGoogleAuthService
{
    Task<AuthSuccessDTO> GoogleRegisterAsync(string authorizationCode);
    Task<AuthSuccessDTO> GoogleLoginAsync(string authorizationCode);
}
