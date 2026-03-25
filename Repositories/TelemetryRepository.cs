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
        public async Task<IEnumerable<TelemetryRecord>> GetData(Guid deviceId,DateTimeOffset fromDate,DateTimeOffset toDate)
        {
            return await _context.TelemetryRecords.Where(r => r.DeviceId == deviceId && r.Timestamp >= fromDate && r.Timestamp <= toDate).ToListAsync();
        }

        public async Task<TelemetryRecord?> GetLatestData(Guid deviceId)
        {
            var data= await _context.TelemetryRecords.Where(r => r.DeviceId == deviceId).ToListAsync();
            if (data == null || data.Count == 0)
                    return null;
            //Sqlite does not support OrderByDescending with DateTimeOffset, so we need to order in memory
            return await Task.FromResult(data.OrderByDescending(r => r.Timestamp).FirstOrDefault());
        }

        public async Task InsertData(TelemetryRecord telemetryRecord)
        {
            await _context.TelemetryRecords.AddAsync(telemetryRecord);
            await _context.SaveChangesAsync(); 
        }
    }
}
