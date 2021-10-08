using RyhmatyoBuuttiServer.Models;
using System;
using System.Collections.Generic;

namespace RyhmatyoBuuttiServer.Repositories
{
    public interface IUserRepository
    {
        User findUser(long id);
        User findUserByEmail(string email);
        User FindUserByName(string name);
        void AddUser(User newUser);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Boolean doesEmailExist(string email);
        Boolean doesUsernameExist(string username);
        public IEnumerable<User> getAllUsers();
        public User finduserBySteamId(string steamId);
        public User ReturnGamesOfUser(long id);
        User ReturnFriendsOfuser(long id);
        void AddFriend(Friend newFriend);
        Friend GetById(long friendsId, long usersId);
        void DeleteFriend(Friend friend);
    }
}