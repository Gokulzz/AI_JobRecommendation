using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.DAL.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Resumes_resumeId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_resumeId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "resumeId",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_resumeId",
                table: "UserProfiles",
                column: "resumeId",
                unique: true,
                filter: "[resumeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Resumes_resumeId",
                table: "UserProfiles",
                column: "resumeId",
                principalTable: "Resumes",
                principalColumn: "resumeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Resumes_resumeId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_resumeId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "resumeId",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_resumeId",
                table: "UserProfiles",
                column: "resumeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Resumes_resumeId",
                table: "UserProfiles",
                column: "resumeId",
                principalTable: "Resumes",
                principalColumn: "resumeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
