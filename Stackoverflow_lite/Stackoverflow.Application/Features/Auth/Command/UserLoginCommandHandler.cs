using ErrorOr;
using MediatR;
using Stackoverflow.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.Auth.Command
{

    public class UserLoginCommandHandler
   : IRequestHandler<UserLoginCommand, ErrorOr<string>>
    {


        private readonly IIdentityService _identityService;



        public UserLoginCommandHandler(
        IIdentityService identityService)
        {

            _identityService = identityService;

        }



        public async Task<ErrorOr<string>> Handle(
        UserLoginCommand request,
        CancellationToken cancellationToken)
        {


            return await _identityService.LoginAsync(
            request.Email,
            request.Password,
            cancellationToken);

        }

    }
}
