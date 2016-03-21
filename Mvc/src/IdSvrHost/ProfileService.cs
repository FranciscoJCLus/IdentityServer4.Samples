using IdentityServer4.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Core.Models;
using IdentityServer4.Core.Extensions;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Core.Services.InMemory;

namespace IdSvrHost2
{
    public class ProfileService : IProfileService
    {
        InMemoryUser[] _users = new InMemoryUser[]
        {
            new InMemoryUser()
            {
                Subject = "bob",
                Enabled = true,
                Password="bob",
                Username = "bob"
            }
        };

        public ProfileService() { }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var query =
                from u in _users
                where u.Subject == context.Subject.GetSubjectId()
                select u;
            var user = query.Single();

            var claims = new List<Claim>{
                new Claim(JwtClaimTypes.Subject, user.Subject),
            };

            claims.AddRange(user.Claims);
            if (!context.AllClaimsRequested)
            {
                claims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
            }

            context.IssuedClaims = claims;

            return Task.FromResult(0);

            //throw new NotImplementedException();
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);
            //throw new NotImplementedException();
        }
    }
}
