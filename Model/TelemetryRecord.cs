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
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
