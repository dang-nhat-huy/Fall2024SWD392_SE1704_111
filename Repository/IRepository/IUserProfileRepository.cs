﻿using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IUserProfileRepository :IGenericRepository<UserProfile>
    {
        Task<List<UserProfile>> getAllUserProfile();

        Task<User?> GetUserByCurrentId(int userId);
    }
}
