using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleTelemetry.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "telemetry_record",
                columns: table => new
                {
                    device_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    engine_rpm = table.Column<int>(type: "INTEGER", nullable: false),
                    fuel_level_percentage = table.Column<decimal>(type: "NUMERIC", nullable: false),
                    latitude = table.Column<double>(type: "REAL", nullable: false),
                    longitude = table.Column<double>(type: "REAL", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telemetry_record", x => new { x.device_id, x.timestamp });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "telemetry_record");
        }
    }
}
