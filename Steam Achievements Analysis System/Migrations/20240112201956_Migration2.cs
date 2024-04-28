using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steam_Achievements_Analysis_System.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AchievementPercentages_Achievements",
                table: "AchievementPercentages");

            migrationBuilder.DropForeignKey(
                name: "FK__Achieveme__AppId__3A81B327",
                table: "Achievements");

            migrationBuilder.AddForeignKey(
                name: "FK_AchievementPercentages_Achievements",
                table: "AchievementPercentages",
                column: "AchievementId",
                principalTable: "Achievements",
                principalColumn: "AchievementId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Achieveme__AppId__3A81B327",
                table: "Achievements",
                column: "AppId",
                principalTable: "Games",
                principalColumn: "AppId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AchievementPercentages_Achievements",
                table: "AchievementPercentages");

            migrationBuilder.DropForeignKey(
                name: "FK__Achieveme__AppId__3A81B327",
                table: "Achievements");

            migrationBuilder.AddForeignKey(
                name: "FK_AchievementPercentages_Achievements",
                table: "AchievementPercentages",
                column: "AchievementId",
                principalTable: "Achievements",
                principalColumn: "AchievementId");

            migrationBuilder.AddForeignKey(
                name: "FK__Achieveme__AppId__3A81B327",
                table: "Achievements",
                column: "AppId",
                principalTable: "Games",
                principalColumn: "AppId");
        }
    }
}
