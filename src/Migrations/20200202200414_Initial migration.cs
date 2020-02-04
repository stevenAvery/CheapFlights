using Microsoft.EntityFrameworkCore.Migrations;

namespace CheapFlights.Migrations
{
    public partial class Initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    IataCode = table.Column<string>(maxLength: 3, nullable: false),
                    City = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.IataCode);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginIataCode = table.Column<string>(nullable: false),
                    DestinationIataCode = table.Column<string>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Airports_DestinationIataCode",
                        column: x => x.DestinationIataCode,
                        principalTable: "Airports",
                        principalColumn: "IataCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flights_Airports_OriginIataCode",
                        column: x => x.OriginIataCode,
                        principalTable: "Airports",
                        principalColumn: "IataCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationIataCode",
                table: "Flights",
                column: "DestinationIataCode");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_OriginIataCode",
                table: "Flights",
                column: "OriginIataCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Airports");
        }
    }
}
