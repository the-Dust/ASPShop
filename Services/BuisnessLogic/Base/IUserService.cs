﻿using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BuisnessLogic.Base
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
    }
}
