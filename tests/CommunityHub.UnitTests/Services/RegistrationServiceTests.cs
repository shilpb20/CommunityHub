using System;
using System.Threading.Tasks;
using AppComponents.Repository.Abstraction;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;


namespace CommunityHub.UnitTests.Services
{
    public class RegistrationServiceTests
    {
        private readonly Mock<ILogger<IRegistrationService>> _mockLogger;
        private readonly Mock<IRepository<RegistrationRequest, ApplicationDbContext>> _mockRepository;
        private readonly RegistrationService _registrationService;

        public RegistrationServiceTests()
        {
            _mockLogger = new Mock<ILogger<IRegistrationService>>();
            _mockRepository = new Mock<IRepository<RegistrationRequest, ApplicationDbContext>>();
            _registrationService = new RegistrationService(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task CreateRequestAsync_ShouldReturnNull_WhenRegistrationDataIsNull()
        {
            // Act
            var result = await _registrationService.CreateRequestAsync(null);

            // Assert
            Assert.Null(result);
            _mockLogger.Verify(
                log => log.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Registration data null")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once);
        }

        [Fact]
        public async Task CreateRequestAsync_ShouldSaveRegistrationRequest_WhenValidDataProvided()
        {
            // Arrange
            var registrationData = new RegistrationData
            {
                UserDetails = new UserDetailsCreateDto { FullName = "John Doe" }
            };

            var expectedRequest = new RegistrationRequest
            {
                RegistrationData = JsonConvert.SerializeObject(registrationData),
                CreatedAt = DateTime.UtcNow,
                RegistrationStatus = "Pending"
            };

            _mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<RegistrationRequest>()))
                .ReturnsAsync(expectedRequest);

            // Act
            var result = await _registrationService.CreateRequestAsync(registrationData);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pending", result.RegistrationStatus);
            Assert.Equal(JsonConvert.SerializeObject(registrationData), result.RegistrationData);

            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<RegistrationRequest>()), Times.Once);
        }

        [Fact]
        public async Task CreateRequestAsync_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            var registrationData = new RegistrationData
            {
                UserDetails = new UserDetailsCreateDto { FullName = "Jane Doe" }
            };

            _mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<RegistrationRequest>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _registrationService.CreateRequestAsync(registrationData));
            Assert.Equal("Database error", exception.Message);

            _mockLogger.Verify(
                log => log.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Error occurred while creating registration request")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once);
        }
    }
}

