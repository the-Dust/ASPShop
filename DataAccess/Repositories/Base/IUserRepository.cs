using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Base
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        IEnumerable<User> GetUsers(Expression<Func<User, bool>> func);
    }
}
