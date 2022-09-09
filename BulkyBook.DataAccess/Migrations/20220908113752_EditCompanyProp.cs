using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBookWeb.Migrations
{
    public partial class EditCompanyProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Companies",
                newName: "Province");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Companies",
                newName: "State");
        }
    }
}
