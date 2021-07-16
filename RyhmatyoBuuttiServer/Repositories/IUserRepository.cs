using RyhmatyoBuuttiServer.Models;
using System;

namespace RyhmatyoBuuttiServer.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User newUser);
        Boolean doesEmailExist(User user);
        Boolean doesUsernameExist(User user);
    }
}