using RyhmatyoBuuttiServer.Models;
using System.Collections.Generic;

namespace RyhmatyoBuuttiServer.Repositories
{
    public interface IGameRepository
    {
        void AddGame(Game game);
        Game FindGame(int steamId);
        void UpdateGame(Game game);
        IEnumerable<Game> GetAllGames();
        Publisher FindPublisher(string name);
        Genre FindGenre(string description);
        Developer FindDeveloper(string developer);
        Game ReturnGameById(int appId);
        UserGame FindUserGame(int SteamId);
        void AddUserGame(UserGame userGame);
        void UpdateUserGame(UserGame userGame);
    }
}
