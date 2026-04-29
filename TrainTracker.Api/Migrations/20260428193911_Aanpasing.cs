using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class Aanpasing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Stations_ArrivalStationId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Stations_DepartureStationId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Trains_TrainId",
                table: "Trips");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Stations_ArrivalStationId",
                table: "Trips",
                column: "ArrivalStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Stations_DepartureStationId",
                table: "Trips",
                column: "DepartureStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Trains_TrainId",
                table: "Trips",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Stations_ArrivalStationId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Stations_DepartureStationId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Trains_TrainId",
                table: "Trips");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Stations_ArrivalStationId",
                table: "Trips",
                column: "ArrivalStationId",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Stations_DepartureStationId",
                table: "Trips",
                column: "DepartureStationId",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Trains_TrainId",
                table: "Trips",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
