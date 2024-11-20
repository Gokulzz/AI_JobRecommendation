using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.DAL.Migrations
{
    public partial class updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SkillName",
                table: "ResumeSkills",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<bool>(
                name: "isApplied",
                table: "JobRecommendations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isViewed",
                table: "JobRecommendations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "priorityLevel",
                table: "JobRecommendations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "relevanceScore",
                table: "JobRecommendations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isApplied",
                table: "JobRecommendations");

            migrationBuilder.DropColumn(
                name: "isViewed",
                table: "JobRecommendations");

            migrationBuilder.DropColumn(
                name: "priorityLevel",
                table: "JobRecommendations");

            migrationBuilder.DropColumn(
                name: "relevanceScore",
                table: "JobRecommendations");

            migrationBuilder.AlterColumn<string>(
                name: "SkillName",
                table: "ResumeSkills",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
