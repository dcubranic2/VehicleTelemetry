using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VehicleTelemetry.Data.Persistance;
using VehicleTelemetry.Interfaces;
using VehicleTelemetry.Model;

namespace VehicleTelemetry.Repositories
{
    public class TelemetryRepository : ITelemetryRepositoryPattern
    {
        private readonly TelemetryDBContext _context;
        private readonly ILogger<TelemetryRepository> _logger;

        public TelemetryRepository(TelemetryDBContext context, ILogger<TelemetryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<TelemetryRecord>> GetAllData()
        {
            return await _context.TelemetryRecords.ToListAsync();
        }
        public async Task<IEnumerable<TelemetryRecord>> GetData(Guid deviceId,DateTime fromDate,DateTime toDate)
        {
            return await _context.TelemetryRecords.Where(r => r.DeviceId == deviceId && r.CreatedAt >= fromDate && r.CreatedAt <= toDate).ToListAsync();
        }

        public async Task<TelemetryRecord?> GetLatestData(Guid deviceId)
        {
            //Sqlite does not support OrderByDescending with DateTimeOffset data type, so we need to order by CreatedAd which is DateTime data type
            var data = await _context.TelemetryRecords.Where(r => r.DeviceId == deviceId).OrderByDescending(r => r.CreatedAt).FirstOrDefaultAsync();
            return data;
        }

        public async Task InsertData(TelemetryRecord telemetryRecord)
        {
            await _context.TelemetryRecords.AddAsync(telemetryRecord);
            await _context.SaveChangesAsync(); 
        }
    }
}
