using Microsoft.EntityFrameworkCore;
using VehicleTelemetry.Data.Persistance;

namespace VehicleTelemetry.Workers
{
    public class CloudSend : BackgroundService
    {
        private readonly ILogger<CloudSend> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CloudSend(ILogger<CloudSend> logger,IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    _logger.LogInformation("CloudSend worker running at: {time}", DateTimeOffset.Now);
                    await SendDataToCloudAsync(stoppingToken);
                }
            }
        }

        private async Task SendDataToCloudAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
                return;
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TelemetryDBContext>();

            var data = await dbContext.TelemetryRecords.OrderByDescending(r => r.CreatedAt).Take(5).ToListAsync();
            data.ForEach(r =>
            {
                _logger.LogInformation("Sending data to cloud for Device {DeviceId}: Timestamp={Timestamp}, EngineRpm={EngineRpm}, FuelLevelPercentage={FuelLevelPercentage}, Latitude={Latitude}, Longitude={Longitude}",
                    r.DeviceId, r.Timestamp, r.EngineRpm, r.FuelLevelPercentage, r.Latitude, r.Longitude);
            });
        }
    }
}
