using RyhmatyoBuuttiServer.Models;
using System.Linq;

namespace RyhmatyoBuuttiServer.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }

        public void AddGame(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
        }
       
        public Game FindGame(int SteamId)
        {
            return _context.Games.FirstOrDefault(g => g.SteamId == SteamId);
        }

    }

}
