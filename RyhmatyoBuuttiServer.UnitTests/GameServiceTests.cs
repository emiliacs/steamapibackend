using Moq;
using NUnit.Framework;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using RyhmatyoBuuttiServer.Services;
using System.Collections.Generic;

namespace RyhmatyoBuuttiServer.UnitTests
{
    [TestFixture]
    class GameServiceTests
    {

        private readonly GameService _gameService;
        private readonly Mock<IGameRepository> _mockGameRepository = new Mock<IGameRepository>();

        public GameServiceTests()
        {
            _gameService = new GameService(_mockGameRepository.Object);
        }

        private readonly Game mockGame = new Game();
        private GameDetailsDto GetTestGameDetailsDto()
        {
            var gameDetails = new GameDetailsDto();
            gameDetails.ImageUrl = "imageurl";
            gameDetails.ReleaseDate = new GameReleaseDateDto { ComingSoon = false, Date = "01.01.2020" };
            gameDetails.Developers = new List<string> {"testDeveloper"};
            gameDetails.Genres = new List<Genre> { new Genre { Id = 0, Description = "testGenre" } };
            gameDetails.Publishers = new List<string> {"testPublisher"};
            return gameDetails;
        }
 
        [Test]
        public void AddImageUrl_WithCorrectUrl_ReturnsUrl()
        {
            var gameDetailsDto = GetTestGameDetailsDto();
            _gameService.AddImageUrl(mockGame, gameDetailsDto);
            Assert.IsNotNull(mockGame.ImageUrl);
            Assert.AreEqual(mockGame.ImageUrl, gameDetailsDto.ImageUrl);
        }

        [TestCase(2020)]
        public void AddRelaseYear_AddingValidReleaseyear_IsNotNull(int releaseYear)
        {
            var gameDetailsDto = GetTestGameDetailsDto();
            _gameService.AddReleaseYear(mockGame, gameDetailsDto);
            Assert.AreEqual(releaseYear, mockGame.ReleaseYear);
            Assert.IsNotNull(mockGame.ReleaseYear);
        }

        [TestCase(1)]
        public void AddDevelopers_WhenAddingValidDevelopers_IsNotNull(int developersCount)
        {
            var gameDetailsDto = GetTestGameDetailsDto();
            _gameService.AddDevelopers(mockGame, gameDetailsDto);
            Assert.AreEqual(developersCount, mockGame.Developers.Count);
            Assert.IsNotNull(mockGame.Developers);
        }

        [TestCase(1)]
        public void AddGenres_WhenAddingValidGenres_IsNotNull(int genresCount)
        {
            var gameDetailsDto = GetTestGameDetailsDto();
            _gameService.AddGenres(mockGame, gameDetailsDto);
            Assert.AreEqual(genresCount, mockGame.Genres.Count);
            Assert.IsNotNull(mockGame.Genres);
        }

        [TestCase(1)]
        public void AddPublishers_WhenAddingPublishers_IsNotNull(int publishersCount)
        {
            var gameDetailsDto = GetTestGameDetailsDto();
            _gameService.AddPublishers(mockGame, gameDetailsDto);
            Assert.AreEqual(publishersCount, mockGame.Publishers.Count);
            Assert.IsNotNull(mockGame.Publishers);
        }



    }
}


