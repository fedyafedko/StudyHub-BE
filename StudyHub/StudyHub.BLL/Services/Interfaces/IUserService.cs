﻿using StudyHub.Common.DTO.User;
using StudyHub.Common.Requests;

namespace StudyHub.BLL.Services.Interfaces;

public interface IUserService
{
    Task<PageList<UserDTO>> GetStudents(SearchRequest request);
}
