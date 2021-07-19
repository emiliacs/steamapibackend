using RyhmatyoBuuttiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Boolean doesEmailExist(User user)
        {
           return _context.Users.Any(u => u.Email == user.Email);
        }

        public Boolean doesUsernameExist(User user)
        {
            return _context.Users.Any(u => u.Username == user.Username);
        }

        public List<User> getAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
