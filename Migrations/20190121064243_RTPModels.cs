using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerShareV2.Migrations
{
    public partial class RTPModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RTPModels",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderPromptPay = table.Column<string>(maxLength: 13, nullable: false),
                    ReceiverPromptPay = table.Column<string>(maxLength: 13, nullable: false),
                    RequestedDateTime = table.Column<DateTime>(nullable: false),
                    ActivePeriod = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RTPModels", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RTPModels");
        }
    }
}
