using Microsoft.EntityFrameworkCore;
using RyhmatyoBuuttiServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RyhmatyoBuuttiServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public User findUser(long id)
        {
            return _context.Users.Find(id);
        }

        public User findUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public User FindUserByName(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Username == name);
        }

        public void AddUser(User newUser)
        {
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
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
            return _context.Users.Include(u => u.Games);
        }
        public User finduserBySteamId(string steamId)
        {
            return _context.Users.FirstOrDefault(u => u.SteamId == steamId);
        }
        public User ReturnGamesOfUser(long id)
        {
            return _context.Users.Include(u => u.Games).ThenInclude(Games => Games.Game.Publishers)
                                  .Include(u => u.Games).ThenInclude(Games => Games.Game.Genres)
                                  .Include(u => u.Games).ThenInclude(Games => Games.Game.Publishers).FirstOrDefault(u => u.Id == id);
        }
        public User ReturnFriendsOfuser(long id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            user.Friends = _context.Friends.Where(Friend => Friend.UserEntityId == id).ToArray();
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }
        public void AddFriend(Friend newFriend)
        {
            _context.Friends.Add(newFriend);
            _context.SaveChanges();
        }
        public Friend GetById(long friendsId, long usersId)
        {
            return _context.Friends.FirstOrDefault(i => i.FriendEntityId == friendsId && i.UserEntityId == usersId);
        }
    }
}
