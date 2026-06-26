using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.Auth.Command
{
    public class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
    {
        public UserRegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);
        }

    }
}
