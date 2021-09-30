using Microsoft.EntityFrameworkCore;
using RyhmatyoBuuttiServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace RyhmatyoBuuttiServer.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        private readonly IUserRepository _userRepository;
        public GameRepository(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        public IEnumerable<Game> GetAllGames()
        {
            return _context.Games.Include(u => u.Genres).Include(u => u.Developers).Include(u => u.Genres);
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

        public void UpdateGame(Game game)
        {
            _context.Games.Update(game);
            _context.SaveChanges();
        }
        public Publisher FindPublisher(string name)
        {
            return _context.Publishers.FirstOrDefault(p => p.Name == name);
        }

        public Genre FindGenre(string description)
        {
            return _context.Genres.FirstOrDefault(g => g.Description == description);
        }

        public Developer FindDeveloper(string developer)
        {
            return _context.Developers.FirstOrDefault(d => d.Developers == developer);
        }

        public Game ReturnGameById(int appId)
        {
            return _context.Games.Include(g => g.Developers).Include(g => g.Genres).Include(g => g.Publishers).FirstOrDefault(g => g.SteamId == appId);
        }

        public UserGame FindUserGame(int SteamId)
        {
            return _context.UserGames.FirstOrDefault(g => g.Game.SteamId == SteamId);
        }
        public void AddUserGame(UserGame userGame)
        {
            _context.UserGames.Add(userGame);
            _context.SaveChanges();
        }
        public void UpdateUserGame(UserGame userGame)
        {
            _context.UserGames.Update(userGame);
            _context.SaveChanges();
        }

    }

}
