using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamA.CustomerAccounts.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerAccounts",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Postcode = table.Column<string>(nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    LoggedOnAt = table.Column<DateTime>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    CanPurchase = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsDeleteRequested = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAccounts", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAccounts");
        }
    }
}
