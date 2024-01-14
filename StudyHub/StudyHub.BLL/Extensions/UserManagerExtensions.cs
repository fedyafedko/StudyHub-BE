﻿using Microsoft.AspNetCore.Identity;
using StudyHub.Entities;

namespace StudyHub.BLL.Extensions;
public static class UserManagerExtensions
{
    public async static Task<User?> FindByIdAsync(this UserManager<User> userManager, Guid id)
    {
        return await userManager.FindByIdAsync(id.ToString());
    }
}
