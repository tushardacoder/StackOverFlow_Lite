using Stackoverflow.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Infrastructure.Security
{
    public interface IJwtService
    {

        string GenerateToken(ApplicationUser user);

    }
}
