
using ErrorOr;
using MediatR;
using Stackoverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.Auth.Command
{
    public class UserRegisterCommand : IRequest<ErrorOr<Guid>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
