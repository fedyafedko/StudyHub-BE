﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudyHub.Common.Exceptions;
using System.Net;

namespace StudyHub.Middlewares;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            UserManagerException => new BadRequestObjectResult(context.Exception.Message),
            NotFoundException => new NotFoundObjectResult(context.Exception.Message),
            InvalidCredentialsException => new UnauthorizedObjectResult(context.Exception.Message),
            InvalidSecurityAlgorithmException => new BadRequestObjectResult(context.Exception.Message),
            TokenValidatorException => new BadRequestObjectResult(context.Exception.Message),
            ExpiredError => new BadRequestObjectResult(context.Exception.Message),
            IncorrectParametersError => new BadRequestObjectResult(context.Exception.Message),
            FluentValidation.ValidationException => new BadRequestObjectResult(context.Exception.Message),
            _ => new ObjectResult(new { error = $"An unexpected error occurred: {context.Exception.Message}" })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            }
        };
    }
}
