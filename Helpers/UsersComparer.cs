using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;

namespace TwitterClone.Helpers
{
    public class UsersComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return String.Equals(x.UserName, y.UserName);
        }

        public int GetHashCode(User obj)
        {
            throw new NotImplementedException();
        }
    }
}