using Microsoft.AspNetCore.Mvc;
using VehicleTelemetry.DTOS;
using VehicleTelemetry.Interfaces;
using VehicleTelemetry.Model;

namespace VehicleTelemetry.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TelemetryController : ControllerBase
    {

        private readonly ILogger<TelemetryController> _logger;
        private readonly ITelemetryService _telemetryService;

        public TelemetryController(ILogger<TelemetryController> logger, ITelemetryService telemetryService)
        {
            _logger = logger;
            _telemetryService = telemetryService;
        }

        [HttpGet("{deviceId}/latest")]
        async public Task<IActionResult> GetLatest(Guid deviceId)
        {
            _logger.LogInformation("Received request for latest telemetry data for device {DeviceId} at {Time}", deviceId, DateTimeOffset.UtcNow);
            var data=await _telemetryService.GetLatestData(deviceId);
            if (data == null) 
                return NotFound();
            return Ok(data);
        }

        [HttpPost]
        async public Task<IActionResult> SaveTelemetryData([FromBody] InputTelemetryDTO telemetryRecord)
        {
            _logger.LogInformation("Received telemetry data request at {Time}", DateTimeOffset.UtcNow);
            await _telemetryService.InsertData(telemetryRecord);
            return Ok();
        }
    }
}
