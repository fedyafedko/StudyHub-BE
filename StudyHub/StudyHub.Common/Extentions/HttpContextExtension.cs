﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RealtorAPI.Extensions;

public static class HttpContextExtension
{
    public static Guid GetUserId(this HttpContext context)
    {
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == "id");

        if (claim == null)
            throw new Exception("Unauthorized");

        return new Guid(claim.Value);
    }

    public static string GetUserRole(this HttpContext context)
    {
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        if (claim == null)
            throw new Exception("Unauthorized");

        return claim.Value;
    }
}