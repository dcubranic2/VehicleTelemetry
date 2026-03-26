using System.ComponentModel.DataAnnotations;

namespace VehicleTelemetry.DTOS
{
    public class TelemetryDTO
    {
        
        public Guid DeviceId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        
        public int EngineRpm { get; set; }
        
        public decimal FuelLevelPercentage { get; set; }
 
        public double Latitude { get; set; }
       
        public double Longitude { get; set; }
    }
}
