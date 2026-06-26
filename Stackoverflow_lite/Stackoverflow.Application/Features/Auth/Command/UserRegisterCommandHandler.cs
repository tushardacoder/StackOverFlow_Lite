using ErrorOr;
using MediatR;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Application.Features.Auth.Command;
using Stackoverflow.Domain.Entities;


public class UserRegisterCommandHandler
: IRequestHandler<UserRegisterCommand, ErrorOr<Guid>>
{


    private readonly IIdentityService _identityService;

    private readonly IUserRepository _repository;



    public UserRegisterCommandHandler(
    IIdentityService identityService,
    IUserRepository repository)
    {

        _identityService = identityService;

        _repository = repository;

    }



    public async Task<ErrorOr<Guid>> Handle(
    UserRegisterCommand request,
    CancellationToken cancellationToken)
    {


        var userId =
        await _identityService.CreateUserAsync(
        request.Email,
        request.Password,
        cancellationToken);

        if (userId.IsError)
        {
            return userId.Errors;
        }

        var profile = new UserProfile
        {

            Id = Guid.NewGuid(),

            UserId = userId.Value,

            ReputationScore = 0

        };



        await _repository.AddAsync(
        profile,
        cancellationToken);
      

        return profile.Id;

    }


}