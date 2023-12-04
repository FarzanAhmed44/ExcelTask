using Microsoft.EntityFrameworkCore.Migrations;

namespace Excel_Demo_2.Migrations
{
    public partial class init22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "peerGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Particular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Empty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustryQuartile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeerGroupAverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeerGroupMedian = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeerGroupMin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeerGroupMax = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peerGroups", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "peerGroups");
        }
    }
}
