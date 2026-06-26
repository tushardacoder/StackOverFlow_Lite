using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Contracts
{
    //public interface IIdentityService
    //{
    //    Task<Guid> CreateUserAsync(string email, string password, CancellationToken cancellationToken);
    //    Task<ErrorOr<string>> LoginAsyn(string email, string password, CancellationToken cancellationToken);


    public interface IIdentityService
    {
        Task<ErrorOr<Guid>> CreateUserAsync(
       string email,
       string password,
       CancellationToken cancellationToken);



        Task<ErrorOr<string>> LoginAsync(
         string email,
         string password,
         CancellationToken cancellationToken);
    }

    }
