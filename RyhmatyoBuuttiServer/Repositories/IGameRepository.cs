using RyhmatyoBuuttiServer.Models;
using System.Collections.Generic;

namespace RyhmatyoBuuttiServer.Repositories
{
    public interface IGameRepository
    {
        public void AddGame(Game game);

        public Game FindGame(int steamId);

    }
}
