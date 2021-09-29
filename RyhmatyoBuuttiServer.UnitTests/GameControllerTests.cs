using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RyhmatyoBuuttiServer.Controllers;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using RyhmatyoBuuttiServer.Services;
using System.Collections.Generic;

namespace RyhmatyoBuuttiServer.UnitTests
{
    [TestFixture]
    class GameControllerTests
    {
        private readonly Mock<IGameRepository> _mockGameRepository = new Mock<IGameRepository>();
        private readonly Mock<IGameService> _mockGameService = new Mock<IGameService>();
        private readonly GameController _gameController;

        public GameControllerTests()
        {
            _gameController = new GameController(_mockGameRepository.Object, _mockGameService.Object);
        }

        [Test]
        public void GetAllGames_FromGameRepository_ReturnsOk()
        {
            _mockGameRepository.Setup(r => r.GetAllGames()).Returns(GetGames());
            var result = _gameController.GetAllGames();
            var objectResult = result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetGameByAppId_WithCorrectGameAppId_ReturnsOk(int appId)
        {
            _mockGameRepository.Setup(s => s.ReturnGameById(appId)).Returns(GetGames().Find(g => g.SteamId == appId));
            var result = _gameController.GetGameByAppId(1);
            var objectResult = result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, objectResult.StatusCode);
        }
        private List<Game> GetGames()
        {
            var games = new List<Game>
            {
                new Game {Id = 1, SteamId = 1, Name = "TestExampleGame"},
                new Game {Id = 2, SteamId = 2, Name = "TestGame2"}
            };
            return games;
        }
    }
}


