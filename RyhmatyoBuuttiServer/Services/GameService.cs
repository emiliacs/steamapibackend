using RyhmatyoBuuttiServer.Models;
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
    }
    public class GameService : IGameService
    {
        public void AddReleaseYear(Game game, GameDetailsDto gameDetailsDto)
        {
            game.ReleaseYear = Int16.Parse(gameDetailsDto.ReleaseDate.Date.Substring(gameDetailsDto.ReleaseDate.Date.Length - 4));
        }
        public void AddDevelopers(Game game, GameDetailsDto gameDetailsDto)
        {
            List<Developer> developers = new List<Developer>();
            foreach (var item in gameDetailsDto.Developers)
            {
                Developer newDeveloper = new Developer();
                newDeveloper.Developers = item;
                developers.Add(newDeveloper);
            }
            game.Developers = developers;
        }
        public void AddGenres(Game game, GameDetailsDto gameDetailsDto)
        {
            List<Genre> genres = new List<Genre>();
            foreach (var item in gameDetailsDto.Genres)
            {
                Genre genre = new Genre();
                genre.Description = item.Description;
                genres.Add(genre);
            }
            game.Genres = genres;
        }
        public void AddPublishers(Game game, GameDetailsDto gameDetailsDto)
        {
            List<Publisher> publishers = new List<Publisher>();
            foreach (var item in gameDetailsDto.Publishers)
            {
                Publisher publisher = new Publisher();
                publisher.Name = item;
                publishers.Add(publisher);
            }
            game.Publishers = publishers;
        }

    }
}
