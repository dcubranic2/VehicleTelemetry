using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using VehicleTelemetry.DTOS;
using VehicleTelemetry.Interfaces;
using VehicleTelemetry.Model;
using VehicleTelemetry.Services;

namespace VehicleTelemetry.Tests;

[TestFixture]
public class TelemetryServiceTests
{
    private Mock<ITelemetryRepositoryPattern> _repositoryMock;
    private Mock<ILogger<TelemetryService>> _loggerMock;
    private ITelemetryService _service;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<ITelemetryRepositoryPattern>();
        _loggerMock = new Mock<ILogger<TelemetryService>>();
        _service = new TelemetryService(_repositoryMock.Object, _loggerMock.Object);
    }

    #region GetAllData

    [Test]
    public async Task GetAllData_ReturnsAllRecordsMappedToDtos()
    {
        // Arrange
        var deviceId = Guid.NewGuid();
        var timestamp = DateTimeOffset.UtcNow;
        var records = new List<TelemetryRecord>
        {
            new TelemetryRecord
            {
                DeviceId = deviceId,
                Timestamp = timestamp,
                EngineRpm = 3000,
                FuelLevelPercentage = 75.5m,
                Latitude = 45.815m,
                Longitude = 15.982m
            },
            new TelemetryRecord
            {
                DeviceId = Guid.NewGuid(),
                Timestamp = timestamp.AddMinutes(-5),
                EngineRpm = 1500,
                FuelLevelPercentage = 50.0m,
                Latitude = 46.0m,
                Longitude = 16.0m
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllData())
            .ReturnsAsync(records);

        // Act
        var result = (await _service.GetAllData()).ToList();

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].DeviceId, Is.EqualTo(records[0].DeviceId));
        Assert.That(result[0].Timestamp, Is.EqualTo(records[0].Timestamp));
        Assert.That(result[0].EngineRpm, Is.EqualTo(records[0].EngineRpm));
        Assert.That(result[0].FuelLevelPercentage, Is.EqualTo(records[0].FuelLevelPercentage));
        Assert.That(result[0].Latitude, Is.EqualTo(records[0].Latitude));
        Assert.That(result[0].Longitude, Is.EqualTo(records[0].Longitude));
    }

    [Test]
    public async Task GetAllData_WhenNoRecords_ReturnsEmptyCollection()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllData())
            .ReturnsAsync(new List<TelemetryRecord>());

        // Act
        var result = await _service.GetAllData();

        // Assert
        Assert.That(result, Is.Empty);
    }

    #endregion

    #region GetData

    [Test]
    public async Task GetData_ReturnsFilteredRecordsMappedToDtos()
    {
        // Arrange
        var deviceId = Guid.NewGuid();
        var fromDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var toDate = new DateTime(2024, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        var timestamp = new DateTimeOffset(2024, 6, 15, 12, 0, 0, TimeSpan.Zero);

        var records = new List<TelemetryRecord>
        {
            new TelemetryRecord
            {
                DeviceId = deviceId,
                Timestamp = timestamp,
                EngineRpm = 4000,
                FuelLevelPercentage = 60.0m,
                Latitude = 45.0m,
                Longitude = 15.0m
            }
        };

        _repositoryMock
            .Setup(r => r.GetData(deviceId, fromDate, toDate))
            .ReturnsAsync(records);

        // Act
        var result = (await _service.GetData(deviceId, fromDate, toDate)).ToList();

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].DeviceId, Is.EqualTo(deviceId));
        Assert.That(result[0].Timestamp, Is.EqualTo(timestamp));
        Assert.That(result[0].EngineRpm, Is.EqualTo(4000));
        Assert.That(result[0].FuelLevelPercentage, Is.EqualTo(60.0m));
        Assert.That(result[0].Latitude, Is.EqualTo(45.0));
        Assert.That(result[0].Longitude, Is.EqualTo(15.0));
    }

    [Test]
    public async Task GetData_WhenNoMatchingRecords_ReturnsEmptyCollection()
    {
        // Arrange
        var deviceId = Guid.NewGuid();
        var fromDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var toDate = new DateTime(2024, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        _repositoryMock
            .Setup(r => r.GetData(deviceId, fromDate, toDate))
            .ReturnsAsync(new List<TelemetryRecord>());

        // Act
        var result = await _service.GetData(deviceId, fromDate, toDate);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetData_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var deviceId = Guid.NewGuid();
        var fromDate = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc);
        var toDate = new DateTime(2024, 3, 31, 23, 59, 59, DateTimeKind.Utc);

        _repositoryMock
            .Setup(r => r.GetData(deviceId, fromDate, toDate))
            .ReturnsAsync(new List<TelemetryRecord>());

        // Act
        await _service.GetData(deviceId, fromDate, toDate);

        // Assert
        _repositoryMock.Verify(r => r.GetData(deviceId, fromDate, toDate), Times.Once);
    }

    #endregion

    #region GetLatestData

    [Test]
    public async Task GetLatestData_WhenRecordExists_ReturnsMappedDto()
    {
        // Arrange
        var deviceId = Guid.NewGuid();
        var timestamp = DateTimeOffset.UtcNow;
        var record = new TelemetryRecord
        {
            DeviceId = deviceId,
            Timestamp = timestamp,
            EngineRpm = 2500,
            FuelLevelPercentage = 80.0m,
            Latitude = 45.5m,
            Longitude = 15.5m
        };

        _repositoryMock
            .Setup(r => r.GetLatestData(deviceId))
            .ReturnsAsync(record);

        // Act
        var result = await _service.GetLatestData(deviceId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.DeviceId, Is.EqualTo(deviceId));
        Assert.That(result.Timestamp, Is.EqualTo(timestamp));
        Assert.That(result.EngineRpm, Is.EqualTo(2500));
        Assert.That(result.FuelLevelPercentage, Is.EqualTo(80.0m));
        Assert.That(result.Latitude, Is.EqualTo(45.5));
        Assert.That(result.Longitude, Is.EqualTo(15.5));
    }

    [Test]
    public async Task GetLatestData_WhenNoRecordExists_ReturnsNull()
    {
        // Arrange
        var deviceId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetLatestData(deviceId))
            .ReturnsAsync((TelemetryRecord?)null);

        // Act
        var result = await _service.GetLatestData(deviceId);

        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region InsertData

    [Test]
    public async Task InsertData_CallsRepositoryWithMappedRecord()
    {
        // Arrange
        var input = new InputTelemetryDTO
        {
            DeviceId = Guid.NewGuid(),
            EngineRpm = 3500,
            FuelLevelPercentage = 65.0m,
            Latitude = 44.0m,
            Longitude = 14.0m
        };

        _repositoryMock
            .Setup(r => r.InsertData(It.IsAny<TelemetryRecord>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.InsertData(input);

        // Assert
        _repositoryMock.Verify(r => r.InsertData(It.Is<TelemetryRecord>(t =>
            t.DeviceId == input.DeviceId &&
            t.EngineRpm == input.EngineRpm &&
            t.FuelLevelPercentage == input.FuelLevelPercentage &&
            t.Latitude == input.Latitude &&
            t.Longitude == input.Longitude
        )), Times.Once);
    }

    [Test]
    public async Task InsertData_CallsRepositoryExactlyOnce()
    {
        // Arrange
        var input = new InputTelemetryDTO
        {
            DeviceId = Guid.NewGuid(),
            EngineRpm = 1000,
            FuelLevelPercentage = 100.0m,
            Latitude = 0.0m,
            Longitude = 0.0m
        };

        _repositoryMock
            .Setup(r => r.InsertData(It.IsAny<TelemetryRecord>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.InsertData(input);

        // Assert
        _repositoryMock.Verify(r => r.InsertData(It.IsAny<TelemetryRecord>()), Times.Once);
    }

    #endregion
}
