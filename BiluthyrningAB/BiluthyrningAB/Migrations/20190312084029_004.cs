using Microsoft.EntityFrameworkCore.Migrations;

namespace BiluthyrningAB.Migrations
{
    public partial class _004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SocSecNumber",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SocSecNumber",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
