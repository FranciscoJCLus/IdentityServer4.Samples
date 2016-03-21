using IdentityServer4.Core.Services.InMemory;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdSvrHost.UI.Login
{
    public class LoginService
    {
        //private readonly List<InMemoryUser> _users;
        private InMemoryUser _user;

        //public LoginService(List<InMemoryUser> users)
        public LoginService()
        {
            //_users = users;
        }

        public bool ValidateCredentials(string username, string password)
        {
            //var user = FindByUsername(username);
            //if (user != null)
            //{
            //    return user.Password.Equals(password);
            //}
            //return false;

            //  search for user "username"
            if (username.Equals("bob"))
            {
                //  retrieve claims
                var user = new InMemoryUser()
                {
                    Username = username,
                    Password = "bob",
                    Enabled = true,
                    Subject = username,

                    //  roles
                    Claims = new Claim[]
                    {
                        new Claim("role", "admin")
                    }
                };

                //  bind to authenticate
                if(password.Equals(user.Password))
                {
                    _user = user;
                    return true;
                }
            }
            return false;
        }

        public InMemoryUser FindByUsername(string username)
        {
            //return _users.FirstOrDefault(x=>x.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase));
            return username.Equals("bob") ? _user : null;
        }
    }
}
