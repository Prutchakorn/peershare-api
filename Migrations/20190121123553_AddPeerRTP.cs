using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerShareV2.Migrations
{
    public partial class AddPeerRTP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PeerRTP",
                schema: "PS",
                columns: table => new
                {
                    PeerId = table.Column<long>(nullable: false),
                    RTPId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeerRTP", x => new { x.PeerId, x.RTPId });
                    table.ForeignKey(
                        name: "FK_PeerRTP_Peer_PeerId",
                        column: x => x.PeerId,
                        principalSchema: "PS",
                        principalTable: "Peer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeerRTP_RTPModels_RTPId",
                        column: x => x.RTPId,
                        principalTable: "RTPModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeerRTP_RTPId",
                schema: "PS",
                table: "PeerRTP",
                column: "RTPId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeerRTP",
                schema: "PS");
        }
    }
}
