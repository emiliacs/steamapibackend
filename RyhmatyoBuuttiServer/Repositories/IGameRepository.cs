using RyhmatyoBuuttiServer.Models;

namespace RyhmatyoBuuttiServer.Repositories
{
    public interface IGameRepository
    {
        void AddGame(Game game);
        Game FindGame(int steamId);
        void UpdateGame(Game game);
    }
}
