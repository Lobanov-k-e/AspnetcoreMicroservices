using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    BillingAdress_AddressLine = table.Column<string>(nullable: true),
                    BillingAdress_Country = table.Column<string>(nullable: true),
                    BillingAdress_State = table.Column<string>(nullable: true),
                    BillingAdress_ZipCode = table.Column<string>(nullable: true),
                    Payment_CardName = table.Column<string>(nullable: true),
                    Payment_CardNumber = table.Column<string>(nullable: true),
                    Payment_Expiration = table.Column<string>(nullable: true),
                    Payment_CVV = table.Column<string>(nullable: true),
                    Payment_PaymentMethod = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
