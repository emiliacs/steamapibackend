using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Services
{
    public interface IGameService
    {
        void AddReleaseYear(Game game, GameDetailsDto gameDetailsDto);
        void AddDevelopers(Game game, GameDetailsDto gameDetailsDto);
        void AddGenres(Game game, GameDetailsDto gameDetailsDto);
        void AddPublishers(Game game, GameDetailsDto gameDetailsDto);
        void AddImageUrl(Game game, GameDetailsDto gameDetailsDto);
    }
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public void AddImageUrl(Game game, GameDetailsDto gameDetailsDto)
        {
            game.ImageUrl = gameDetailsDto.ImageUrl;
        }

        public void AddReleaseYear(Game game, GameDetailsDto gameDetailsDto)
        {
            if (!String.IsNullOrEmpty(gameDetailsDto.ReleaseDate.Date))
            {
                game.ReleaseYear = Int16.Parse(gameDetailsDto.ReleaseDate.Date.Substring(gameDetailsDto.ReleaseDate.Date.Length - 4));
            }
            else game.ReleaseYear = 0;

        }
        public void AddDevelopers(Game game, GameDetailsDto gameDetailsDto)
        {
            List<Developer> developers = new List<Developer>();
            foreach (var developer in gameDetailsDto.Developers)
            {
                Developer newDeveloper = new Developer();
                newDeveloper.Developers = developer;
                developers.Add(newDeveloper);
            }
            game.Developers = developers;
        }
        public void AddGenres(Game game, GameDetailsDto gameDetailsDto)
        {
            List<Genre> genres = new List<Genre>();
            foreach (var genre in gameDetailsDto.Genres)
            {
                Genre newGenre = new Genre();
                newGenre.Description = genre.Description;
                genres.Add(newGenre);
            }
            game.Genres = genres;
        }

        public void AddPublishers(Game game, GameDetailsDto gameDetailsDto)
        {
            List<Publisher> publishers = new List<Publisher>();
            foreach (var name in gameDetailsDto.Publishers)
            {
                Publisher publisher = new Publisher();
                publisher.Name = name;
                publishers.Add(publisher);
            }
            game.Publishers = publishers;
        }
    }
}
