using VehicleTelemetry.DTOS;
using VehicleTelemetry.Model;

namespace VehicleTelemetry.Interfaces
{
    public interface ITelemetryService
    {
        Task<IEnumerable<TelemetryDTO>> GetAllData();

        Task<IEnumerable<TelemetryDTO>> GetData(Guid deviceId, DateTime fromDate, DateTime toDate);
        Task<TelemetryDTO?> GetLatestData(Guid deviceId);
        Task InsertData(InputTelemetryDTO telemetryRecord);
    }
}
