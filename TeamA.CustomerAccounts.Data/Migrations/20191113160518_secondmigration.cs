using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamA.CustomerAccounts.Data.Migrations
{
    public partial class secondmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "CustomerAccounts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleteRequested",
                table: "CustomerAccounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomerAccounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleteRequested",
                table: "CustomerAccounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomerAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "CustomerAccounts",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
