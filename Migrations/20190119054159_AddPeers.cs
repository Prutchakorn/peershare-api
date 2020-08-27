using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerShareV2.Migrations
{
    public partial class AddPeers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Peer",
                schema: "PS",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillSplitId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PromptPay = table.Column<string>(maxLength: 13, nullable: false),
                    IsPromptPay = table.Column<bool>(nullable: false),
                    PersonalTotalPrice = table.Column<double>(nullable: false),
                    PersonalNetPrice = table.Column<double>(nullable: false),
                    StatusId = table.Column<long>(nullable: false),
                    PaidDateTime = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserAccountId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peer_Status_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "PS",
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Peer_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalSchema: "PS",
                        principalTable: "UserAccount",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Peer_StatusId",
                schema: "PS",
                table: "Peer",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Peer_UserAccountId",
                schema: "PS",
                table: "Peer",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Peer_Id_BillSplitId",
                schema: "PS",
                table: "Peer",
                columns: new[] { "Id", "BillSplitId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Peer",
                schema: "PS");
        }
    }
}
