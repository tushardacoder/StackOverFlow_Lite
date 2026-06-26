using Microsoft.EntityFrameworkCore;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stackoverflow.Infrastructure.Repositories
{

    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public UserProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            return await _context.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.UserId == userId,
                    cancellationToken);
        }


        public void Update(UserProfile profile)
        {
            _context.Update(profile);
        }
    }
}
