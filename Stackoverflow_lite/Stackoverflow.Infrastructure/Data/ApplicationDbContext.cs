using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Infrastructure.Data
{
    public class ApplicationDbContext
      : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }


        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Vote> Votes { get; set; }


    }
}
