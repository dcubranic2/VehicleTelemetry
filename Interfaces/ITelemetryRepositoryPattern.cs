using VehicleTelemetry.Model;

namespace VehicleTelemetry.Interfaces
{
    public interface ITelemetryRepositoryPattern
    {
        Task<IEnumerable<TelemetryRecord>> GetAllData();

        Task<IEnumerable<TelemetryRecord>> GetData(Guid deviceId, DateTimeOffset fromDate, DateTimeOffset toDate);
        Task<TelemetryRecord?> GetLatestData(Guid deviceId);
        Task InsertData(TelemetryRecord telemetryRecord);
    }
}
