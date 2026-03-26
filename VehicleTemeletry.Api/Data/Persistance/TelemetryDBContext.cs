using Microsoft.EntityFrameworkCore;
using VehicleTelemetry.Model;

namespace VehicleTelemetry.Data.Persistance
{
    public partial class TelemetryDBContext : DbContext
    {
        public TelemetryDBContext(DbContextOptions<TelemetryDBContext> options) : base(options)
        {
        }

        public DbSet<Model.TelemetryRecord> TelemetryRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TelemetryRecord>(entity =>
            {
                entity.ToTable("telemetry_record");
                entity.HasKey(e => new { e.DeviceId, e.Timestamp });

                entity.Property(e => e.DeviceId)
                    .HasColumnName("device_id")
                    .HasColumnType("TEXT")
                    .IsRequired();

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("TEXT")
                    .IsRequired();

                entity.Property(e => e.EngineRpm)
                    .HasColumnName("engine_rpm")
                    .HasColumnType("INTEGER")
                    .IsRequired();

                entity.Property(e => e.FuelLevelPercentage)
                    .HasColumnName("fuel_level_percentage")
                    .HasColumnType("NUMERIC")
                    .IsRequired();

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("REAL")
                    .IsRequired();

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("REAL")
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("TEXT");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
