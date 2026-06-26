using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Domain.Contracts;
using Stackoverflow.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IJwtService _jwt;



        public IdentityService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwt)
        {

            _userManager = userManager;

            _jwt = jwt;

        }



        public async Task<ErrorOr<Guid>> CreateUserAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
        {


            var existingUser = await _userManager.FindByEmailAsync(email);


            if (existingUser != null)
            {
                return Error.Conflict(
                    "User.Exists",
                    "User already exists");
            }



            var user = new ApplicationUser
            {

                Id = Guid.NewGuid(),

                Email = email,

                UserName = email

            };



            var result = await _userManager.CreateAsync(
             user, password);

            if (!result.Succeeded)
            {
                return Error.Conflict(
                    "Something wrong",
                    "Please try again");
            }



            return user.Id;

        }





        public async Task<ErrorOr<string>> LoginAsync(
     string email,
     string password,
     CancellationToken cancellationToken)
        {
            var user = await _userManager
                .FindByEmailAsync(email);

            if (user == null)
            {
                return Error.Validation(
                    "Login.Invalid",
                    "Invalid email or password");
            }

            var result = await _userManager
                .CheckPasswordAsync(user, password);

            if (!result)
            {
                return Error.Validation(
                    "Login.Invalid",
                    "Invalid email or password");
            }

            return _jwt.GenerateToken(user);
        }

    }
}
