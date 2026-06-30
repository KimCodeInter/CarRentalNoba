using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PricingRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CarType = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseDayRental = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    BaseKmPrice = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    EffectiveFrom = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CarType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BookingNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    VehicleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CarType = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerSocialSecurityNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PickupDateTime = table.Column<string>(type: "TEXT", nullable: false),
                    PickupMeterReading = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseDayRental = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    BaseKmPrice = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    ReturnDateTime = table.Column<string>(type: "TEXT", nullable: true),
                    ReturnMeterReading = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentals_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PricingRates_CarType_EffectiveFrom",
                table: "PricingRates",
                columns: new[] { "CarType", "EffectiveFrom" });

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_BookingNumber",
                table: "Rentals",
                column: "BookingNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_VehicleId",
                table: "Rentals",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RegistrationNumber",
                table: "Vehicles",
                column: "RegistrationNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PricingRates");

            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
