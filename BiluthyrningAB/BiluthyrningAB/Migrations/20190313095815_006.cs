using Microsoft.EntityFrameworkCore.Migrations;

namespace BiluthyrningAB.Migrations
{
    public partial class _006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfKilometers",
                table: "Cars",
                newName: "NumberOfDrivenKm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfDrivenKm",
                table: "Cars",
                newName: "NumberOfKilometers");
        }
    }
}
