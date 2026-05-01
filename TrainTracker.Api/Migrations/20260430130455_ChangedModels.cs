using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DelayRecords");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Trains",
                newName: "ToStation");

            migrationBuilder.RenameColumn(
                name: "TrainNumber",
                table: "Trains",
                newName: "Platform");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalTime",
                table: "Trains",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Trains",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DelayMinutes",
                table: "Trains",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureTime",
                table: "Trains",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FromStation",
                table: "Trains",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SavedTrips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromStation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToStation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotifyBeforeMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedTrips", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SavedTrips");

            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "DelayMinutes",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "DepartureTime",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "FromStation",
                table: "Trains");

            migrationBuilder.RenameColumn(
                name: "ToStation",
                table: "Trains",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Platform",
                table: "Trains",
                newName: "TrainNumber");

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArrivalStationId = table.Column<int>(type: "int", nullable: false),
                    DepartureStationId = table.Column<int>(type: "int", nullable: false),
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Platform = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Stations_ArrivalStationId",
                        column: x => x.ArrivalStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trips_Stations_DepartureStationId",
                        column: x => x.DepartureStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trips_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DelayRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    DelayMinutes = table.Column<int>(type: "int", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelayRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DelayRecords_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DelayRecords_TripId",
                table: "DelayRecords",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ArrivalStationId",
                table: "Trips",
                column: "ArrivalStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DepartureStationId",
                table: "Trips",
                column: "DepartureStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TrainId",
                table: "Trips",
                column: "TrainId");
        }
    }
}
