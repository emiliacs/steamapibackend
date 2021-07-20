using RyhmatyoBuuttiServer.Models;
using System;
using System.Collections.Generic;

namespace RyhmatyoBuuttiServer.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User newUser);
        Boolean doesEmailExist(string email);
        Boolean doesUsernameExist(string username);
        public IEnumerable<User> getAllUsers();
    }
}