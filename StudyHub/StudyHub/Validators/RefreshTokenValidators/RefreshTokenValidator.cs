﻿using FluentValidation;
using StudyHub.Common.DTO.AuthDTO;

namespace StudyHub.Validators.RefreshTokenValidators;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();

        RuleFor(x => x.Token)
           .NotEmpty();
    }
}
