using Services.BuisnessLogic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Repositories.Base;

namespace Services.BuisnessLogic
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository = null;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return userRepository.GetUsers();
        }
    }
}
