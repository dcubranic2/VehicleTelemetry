using System.ComponentModel.DataAnnotations;

namespace VehicleTelemetry.DTOS
{
    public class InputTelemetryDTO
    {
        [Required]
        public Guid DeviceId { get; set; }

        [Required]
        [Range(0, 10000)]
        public int EngineRpm { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal FuelLevelPercentage { get; set; }

        [Required]
        [Range(-90, 90)]
        public decimal Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public decimal Longitude { get; set; }
        
    }
}
