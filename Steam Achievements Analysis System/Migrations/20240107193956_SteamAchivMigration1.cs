using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steam_Achievements_Analysis_System.Migrations
{
    /// <inheritdoc />
    public partial class SteamAchivMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    AppId = table.Column<int>(type: "int", nullable: false),
                    GameName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Games__8E2CF7F9B62C1278", x => x.AppId);
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    AchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    AchivmentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Achievem__276330C0B4B2C514", x => x.AchievementId);
                    table.ForeignKey(
                        name: "FK__Achieveme__AppId__3A81B327",
                        column: x => x.AppId,
                        principalTable: "Games",
                        principalColumn: "AppId");
                });

            migrationBuilder.CreateTable(
                name: "AchievementPercentages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Achievem__3214EC072195B46B", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AchievementPercentages_Achievements",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "AchievementId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AchievementPercentages_AchievementId",
                table: "AchievementPercentages",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "UQ_Achievement",
                table: "Achievements",
                columns: new[] { "AppId", "AchivmentName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AchievementPercentages");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
