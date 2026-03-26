using VehicleTelemetry.DTOS;
using VehicleTelemetry.Interfaces;

namespace VehicleTelemetry.Services
{
    public class TelemetryService: ITelemetryService
    {
        readonly ITelemetryRepositoryPattern _repository;
        readonly ILogger<TelemetryService> _logger;

        public TelemetryService(ITelemetryRepositoryPattern repository, ILogger<TelemetryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        async Task<IEnumerable<TelemetryDTO>> ITelemetryService.GetAllData()
        {
            var records = await _repository.GetAllData();

            return await Task.FromResult(records.Select(r => new TelemetryDTO
            {
                DeviceId = r.DeviceId,
                Timestamp = r.Timestamp,
                EngineRpm = r.EngineRpm,
                FuelLevelPercentage = r.FuelLevelPercentage,
                Latitude = r.Latitude,
                Longitude = r.Longitude
            }).ToList());
        }

        async Task<IEnumerable<TelemetryDTO>> ITelemetryService.GetData(Guid deviceId, DateTime fromDate, DateTime toDate)
        {
            // Fetch telemetry data from the repository based on the provided device ID and date range.
            var telemetryData = await _repository.GetData(deviceId, fromDate, toDate);

            return await Task.FromResult(telemetryData.Select(r => new TelemetryDTO
            {
                DeviceId = r.DeviceId,
                Timestamp = r.Timestamp,
                EngineRpm = r.EngineRpm,
                FuelLevelPercentage = r.FuelLevelPercentage,
                Latitude = r.Latitude,
                Longitude = r.Longitude
            }).ToList());
        }

        async Task<TelemetryDTO?> ITelemetryService.GetLatestData(Guid deviceId)
        {
            var data=await _repository.GetLatestData(deviceId);
            if (data == null)
                return null;

            return await Task.FromResult(new TelemetryDTO
            {
                DeviceId = data.DeviceId,
                Timestamp = data.Timestamp,
                EngineRpm = data.EngineRpm,
                FuelLevelPercentage = data.FuelLevelPercentage,
                Latitude = data.Latitude,
                Longitude = data.Longitude
            });
        }

        async Task ITelemetryService.InsertData(InputTelemetryDTO telemetryRecord)
        {
            await _repository.InsertData(new Model.TelemetryRecord
            {
                DeviceId = telemetryRecord.DeviceId,
                EngineRpm = telemetryRecord.EngineRpm,
                FuelLevelPercentage = telemetryRecord.FuelLevelPercentage,
                Latitude = telemetryRecord.Latitude,
                Longitude = telemetryRecord.Longitude
            });
        }
    }
}
