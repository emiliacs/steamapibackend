using RyhmatyoBuuttiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RyhmatyoBuuttiServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersContext _context;

        public UserRepository(UsersContext context)
        {
            _context = context;
        }

        public void AddUser(User newUser)
        {
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public Boolean doesEmailExist(string email)
        {
           return _context.Users.Any(u => u.Email == email);
        }

        public Boolean doesUsernameExist(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public IEnumerable<User> getAllUsers()
        {
            return _context.Users;
        }

        public User findUserToAuthenticate(string email)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email);
        }
    }
}
