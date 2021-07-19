using RyhmatyoBuuttiServer.Models;
using System;
using System.Collections.Generic;

namespace RyhmatyoBuuttiServer.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User newUser);
        Boolean doesEmailExist(User user);
        Boolean doesUsernameExist(User user);
        public IEnumerable<User> getAllUsers();
    }
}