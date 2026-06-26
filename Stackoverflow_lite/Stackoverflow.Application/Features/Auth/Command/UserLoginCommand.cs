using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.Auth.Command
{



    public class UserLoginCommand : IRequest<ErrorOr<string>>
    {

        public string Email { get; set; }

        public string Password { get; set; }

    }
}
