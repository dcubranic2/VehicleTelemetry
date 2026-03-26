using VehicleTelemetry.Model;

namespace VehicleTelemetry.Interfaces
{
    public interface ITelemetryRepositoryPattern
    {
        Task<IEnumerable<TelemetryRecord>> GetAllData();

        Task<IEnumerable<TelemetryRecord>> GetData(Guid deviceId, DateTime fromDate, DateTime toDate);
        Task<TelemetryRecord?> GetLatestData(Guid deviceId);
        Task InsertData(TelemetryRecord telemetryRecord);
    }
}
