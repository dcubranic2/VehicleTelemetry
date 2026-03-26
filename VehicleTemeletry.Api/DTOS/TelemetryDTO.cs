using System.ComponentModel.DataAnnotations;

namespace VehicleTelemetry.DTOS
{
    public class TelemetryDTO
    {
        
        public Guid DeviceId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        
        public int EngineRpm { get; set; }
        
        public decimal FuelLevelPercentage { get; set; }
 
        public decimal Latitude { get; set; }
       
        public decimal Longitude { get; set; }
    }
}
