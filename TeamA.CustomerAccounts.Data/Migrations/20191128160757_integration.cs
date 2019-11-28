using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamA.CustomerAccounts.Data.Migrations
{
    public partial class integration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CustomerAccounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "CustomerAccounts",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CustomerAccounts",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "CustomerAccounts",
                newName: "IsDeleted");
        }
    }
}
