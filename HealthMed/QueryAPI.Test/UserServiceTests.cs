using System;
using System.Threading.Tasks;
using HealthMed.Migrator.Data.Entities;
using HealthMed.QueryAPI.Interfaces.Repositories;
using HealthMed.QueryAPI.Services;
using Moq;
using Xunit;

namespace QueryAPI.Test
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetByLogin_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var login = "testuser";
            var expectedUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Login = login
            };

            _repositoryMock
                .Setup(repo => repo.Get(login))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.Get(login);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser, result);
            _repositoryMock.Verify(repo => repo.Get(login), Times.Once);
        }

        [Fact]
        public async Task GetByLogin_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var login = "nonexistentuser";

            _repositoryMock
                .Setup(repo => repo.Get(login))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.Get(login);

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(repo => repo.Get(login), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedUser = new User
            {
                Id = userId,
                Name = "Test User",
                Login = "testuser"
            };

            _repositoryMock
                .Setup(repo => repo.Get(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.Get(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser, result);
            _repositoryMock.Verify(repo => repo.Get(userId), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _repositoryMock
                .Setup(repo => repo.Get(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.Get(userId);

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(repo => repo.Get(userId), Times.Once);
        }
    }
}
