using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerShareV2.Migrations
{
    public partial class AddBillSplits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PS");

            migrationBuilder.CreateTable(
                name: "BillSplit",
                schema: "PS",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OwnerPromptPay = table.Column<string>(maxLength: 13, nullable: false),
                    TotalPrice = table.Column<double>(nullable: false),
                    NumberOfPeople = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Vat = table.Column<double>(nullable: false),
                    ServiceCharge = table.Column<double>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillSplit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                schema: "PS",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    ColorCode = table.Column<string>(maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                schema: "PS",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 20, nullable: true),
                    LastName = table.Column<string>(maxLength: 20, nullable: true),
                    Password = table.Column<string>(nullable: false),
                    PromptPay = table.Column<string>(maxLength: 13, nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    ProfileImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_PromptPay",
                schema: "PS",
                table: "UserAccount",
                column: "PromptPay",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillSplit",
                schema: "PS");

            migrationBuilder.DropTable(
                name: "Status",
                schema: "PS");

            migrationBuilder.DropTable(
                name: "UserAccount",
                schema: "PS");
        }
    }
}
