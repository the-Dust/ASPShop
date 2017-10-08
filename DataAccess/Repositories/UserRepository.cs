using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories.Base;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    public class UserRepository : AbstractRepository, IUserRepository
    {
        private IQueryable<User> GetUserQuery()
        {
            return context.Users.Include(x => x.Group).Include(x => x.Group.Role);
        }

        public IEnumerable<User> GetUsers()
        {
            return GetUserQuery().ToArray();
        }

        public IEnumerable<User> GetUsers(Expression<Func<User, bool>> func)
        {
            return func != null ? GetUserQuery().Where(func).ToArray() : GetUsers();
        }
    }
}
