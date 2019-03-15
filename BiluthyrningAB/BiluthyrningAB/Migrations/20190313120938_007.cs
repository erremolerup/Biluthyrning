using Microsoft.EntityFrameworkCore.Migrations;

namespace BiluthyrningAB.Migrations
{
    public partial class _007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfKm",
                table: "Rentals",
                newName: "NewNumberKmDriven");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewNumberKmDriven",
                table: "Rentals",
                newName: "NumberOfKm");
        }
    }
}
