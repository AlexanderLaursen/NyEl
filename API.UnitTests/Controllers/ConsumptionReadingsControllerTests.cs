using API.Controllers;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Common.Dtos.ConsumptionReading;
using Common.Enums;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.UnitTests.Controllers
{
    public class ConsumptionReadingsControllerTests
    {
        private readonly Mock<IConsumptionService> _mockConsumptionService;
        private readonly Mock<IConsumerService> _mockConsumerService;
        private readonly Mock<ILogger<ConsumptionReadingsController>> _mockLogger;
        private readonly Mock<ICommonRepository<ConsumptionReading>> _mockCommonRepository;

        public ConsumptionReadingsControllerTests()
        {
            _mockConsumptionService = new Mock<IConsumptionService>();
            _mockConsumerService = new Mock<IConsumerService>();
            _mockLogger = new Mock<ILogger<ConsumptionReadingsController>>();
            _mockCommonRepository = new Mock<ICommonRepository<ConsumptionReading>>();
        }

        // Mock controller to override protected method
        private class TestConsumptionReadingsController : ConsumptionReadingsController
        {
            private readonly int _testConsumerId;

            public TestConsumptionReadingsController(
                Mock<ICommonRepository<ConsumptionReading>> commonRepositoryMock,
                Mock<ILogger<ConsumptionReadingsController>> loggerMock,
                Mock<IConsumptionService> consumptionServiceMock,
                Mock<IConsumerService> consumerServiceMock,
                int testConsumerId)
                : base(consumptionServiceMock.Object,
                      consumerServiceMock.Object,
                      commonRepositoryMock.Object,
                      loggerMock.Object)
            {
                _testConsumerId = testConsumerId;
            }

            protected override async Task<int> GetConsumerId()
            {
                return _testConsumerId;
            }
        }

        #region GetByTimeFrame Tests

        [Fact]
        public async Task GetByTimeframe_ReturnsOk_WhenOk()
        {
            // Arrange
            DateTime startDate = new DateTime(2025, 1, 1);
            TimeframeOptions timeframeOptions = TimeframeOptions.Daily;
            int testConsumerId = 456;
            var expectedReadings = new ConsumptionReadingListDto
            {
                ConsumptionReadings = new List<ConsumptionReadingDto>()
                {
                    new ConsumptionReadingDto { Timestamp = new DateTime(25, 1, 1, 0, 0, 0), Consumption = 1.5m},
                    new ConsumptionReadingDto { Timestamp = new DateTime(25, 1, 1, 1, 0, 0), Consumption = 1.3m},
                    new ConsumptionReadingDto { Timestamp = new DateTime(25, 1, 1, 2, 0, 0), Consumption = 1.8m},
                }
            };

            _mockConsumptionService.Setup(service => service.GetConsumptionReadingsAsync(startDate, timeframeOptions, testConsumerId))
                .ReturnsAsync(expectedReadings);

            var controller = new TestConsumptionReadingsController(
                new Mock<ICommonRepository<ConsumptionReading>>(),
                _mockLogger,
                _mockConsumptionService,
                _mockConsumerService,
                testConsumerId
            );

            // Act
            var result = await controller.GetByTimeframe(startDate, timeframeOptions);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Same(expectedReadings, okResult.Value);
        }

        [Fact]
        public async Task GetByTimeframe_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            DateTime startDate = new DateTime(2025, 1, 1);
            TimeframeOptions timeframeOptions = TimeframeOptions.Daily;
            int testConsumerId = 456;

            _mockConsumptionService.Setup(service => service.GetConsumptionReadingsAsync(startDate, timeframeOptions, testConsumerId))
                .ReturnsAsync((ConsumptionReadingListDto)null);

            var controller = new TestConsumptionReadingsController(
                new Mock<ICommonRepository<ConsumptionReading>>(),
                _mockLogger,
                _mockConsumptionService,
                _mockConsumerService,
                testConsumerId
            );

            // Act
            var result = await controller.GetByTimeframe(startDate, timeframeOptions);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByTimeframe_ReturnsStatusCode500_OnRepositoryException()
        {
            // Arrange
            DateTime startDate = new DateTime(2025, 1, 1);
            TimeframeOptions timeframeOptions = TimeframeOptions.Daily;
            int testConsumerId = 456;

            _mockConsumptionService.Setup(service => service.GetConsumptionReadingsAsync(startDate, timeframeOptions, testConsumerId))
                .ThrowsAsync(new RepositoryException());

            var controller = new TestConsumptionReadingsController(
                new Mock<ICommonRepository<ConsumptionReading>>(),
                _mockLogger,
                _mockConsumptionService,
                _mockConsumerService,
                testConsumerId
            );

            // Act
            var result = await controller.GetByTimeframe(startDate, timeframeOptions);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetByTimeframe_ReturnsStatusCode500_OnException()
        {
            // Arrange
            DateTime startDate = new DateTime(2025, 1, 1);
            TimeframeOptions timeframeOptions = TimeframeOptions.Daily;
            int testConsumerId = 456;

            _mockConsumptionService.Setup(service => service.GetConsumptionReadingsAsync(startDate, timeframeOptions, testConsumerId))
                .ThrowsAsync(new Exception());

            var controller = new TestConsumptionReadingsController(
                new Mock<ICommonRepository<ConsumptionReading>>(),
                _mockLogger,
                _mockConsumptionService,
                _mockConsumerService,
                testConsumerId
            );

            // Act
            var result = await controller.GetByTimeframe(startDate, timeframeOptions);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region Post Tests

        [Fact]
        public async Task Post_ReturnsCreatedAtAction_WhenOk()
        {
            // Arrange
            var createConsumptionReadingDto = new CreateConsumptionReadingDto { Consumption = 10, Timestamp = DateTime.Now };

            _mockConsumptionService.Setup(service => service.AddAsync(It.IsAny<ConsumptionReading>()))
                .Returns(Task.CompletedTask);

            var controller = new ConsumptionReadingsController(
                _mockConsumptionService.Object,
                _mockConsumerService.Object,
                _mockCommonRepository.Object,
                _mockLogger.Object
            );

            // Act
            var result = await controller.Post(createConsumptionReadingDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.Post), createdAtActionResult.ActionName);
            Assert.IsType<ConsumptionReadingDto>(createdAtActionResult.Value);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var createConsumptionReadingDto = new CreateConsumptionReadingDto { Consumption = 10, Timestamp = DateTime.Now };

            var controller = new ConsumptionReadingsController(
                _mockConsumptionService.Object,
                _mockConsumerService.Object,
                _mockCommonRepository.Object,
                _mockLogger.Object
            );

            controller.ModelState.AddModelError("test", "test error");

            // Act
            var result = await controller.Post(createConsumptionReadingDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var createConsumptionReadingDto = new CreateConsumptionReadingDto { Consumption = 10, Timestamp = DateTime.Now };

            _mockConsumptionService.Setup(service => service.AddAsync(It.IsAny<ConsumptionReading>()))
                .ThrowsAsync(new Exception("Test Exception"));

            var controller = new ConsumptionReadingsController(
                _mockConsumptionService.Object,
                _mockConsumerService.Object,
                _mockCommonRepository.Object,
                _mockLogger.Object
            );

            // Act
            var result = await controller.Post(createConsumptionReadingDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region PostRange Tests

        [Fact]
        public async Task PostRange_ReturnsCreatedAtAction_WhenOk()
        {
            // Arrange
            var createConsumptionReadingListDto = new CreateConsumptionReadingListDto
            {
                CreateConsumptionReadingDtos = new List<CreateConsumptionReadingDto>
                {
                    new CreateConsumptionReadingDto { Consumption = 1.2m, Timestamp = DateTime.Now },
                    new CreateConsumptionReadingDto { Consumption = 1.8m, Timestamp = DateTime.Now.AddHours(1) }
                }
            };

            _mockConsumptionService.Setup(service => service.AddRangeAsync(It.IsAny<List<ConsumptionReading>>()))
                .Returns(Task.CompletedTask);

            var controller = new ConsumptionReadingsController(
                _mockConsumptionService.Object,
                _mockConsumerService.Object,
                _mockCommonRepository.Object,
                _mockLogger.Object
            );

            // Act
            var result = await controller.PostRange(createConsumptionReadingListDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.Post), createdAtActionResult.ActionName);
            Assert.IsType<List<ConsumptionReadingDto>>(createdAtActionResult.Value);
        }

        [Fact]
        public async Task PostRange_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var createConsumptionReadingListDto = new CreateConsumptionReadingListDto
            {
                CreateConsumptionReadingDtos = new List<CreateConsumptionReadingDto>
                {
                    new CreateConsumptionReadingDto { Consumption = 10, Timestamp = DateTime.Now },
                    new CreateConsumptionReadingDto { Consumption = 20, Timestamp = DateTime.Now.AddHours(1) }
                }
            };

            var controller = new ConsumptionReadingsController(
                _mockConsumptionService.Object,
                _mockConsumerService.Object,
                _mockCommonRepository.Object,
                _mockLogger.Object
            );

            controller.ModelState.AddModelError("test", "test error");

            // Act
            var result = await controller.PostRange(createConsumptionReadingListDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PostRange_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var createConsumptionReadingListDto = new CreateConsumptionReadingListDto
            {
                CreateConsumptionReadingDtos = new List<CreateConsumptionReadingDto>
                {
                    new CreateConsumptionReadingDto { Consumption = 10, Timestamp = DateTime.Now },
                    new CreateConsumptionReadingDto { Consumption = 20, Timestamp = DateTime.Now.AddHours(1) }
                }
            };

            _mockConsumptionService.Setup(service => service.AddRangeAsync(It.IsAny<List<ConsumptionReading>>()))
                .ThrowsAsync(new Exception("Test Exception"));

            var controller = new ConsumptionReadingsController(
                _mockConsumptionService.Object,
                _mockConsumerService.Object,
                _mockCommonRepository.Object,
                _mockLogger.Object
            );

            // Act
            var result = await controller.PostRange(createConsumptionReadingListDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion
    }
}
