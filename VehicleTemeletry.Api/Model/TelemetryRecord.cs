using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VehicleTelemetry.Model
{
    public class TelemetryRecord
    {
        public Guid DeviceId { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
        public int EngineRpm { get; set; }
        public decimal FuelLevelPercentage { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
