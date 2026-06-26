using Microsoft.EntityFrameworkCore;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure.Data;


namespace Stackoverflow.Infrastructure.Repositories;


public class UserRepository
    : Repository<UserProfile, Guid>, IUserRepository
{

    private readonly ApplicationDbContext _context;


    public UserRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }



    public async Task AddAsync(
        UserProfile profile,
        CancellationToken cancellationToken)
    {

        await _context.UserProfiles
            .AddAsync(
                profile,
                cancellationToken);


        await _context.SaveChangesAsync(
            cancellationToken);
    }



    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {

        // IMPORTANT:
        // Email is in AspNetUsers, not UserProfiles

        return await _context.Users
            .AnyAsync(
                x => x.Email == email,
                cancellationToken);

    }



    public async Task<UserProfile?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {

        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);



        if (user == null)
            return null;



        return await _context.UserProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == user.Id,
                cancellationToken);

    }

}