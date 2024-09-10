using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.DAL.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    resumeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    filePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    uplodedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    workExperience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certifications = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.resumeId);
                });

            migrationBuilder.CreateTable(
                name: "ScrapedJobs",
                columns: table => new
                {
                    ScrapedJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScrapedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapedJobs", x => x.ScrapedJobId);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProficiencyLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    passwordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    verfificationToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    verfiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "ResumeSkills",
                columns: table => new
                {
                    ResumeSkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResumeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProficiencyLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeSkills", x => x.ResumeSkillId);
                    table.ForeignKey(
                        name: "FK_ResumeSkills_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "resumeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobSkills",
                columns: table => new
                {
                    JobSkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ScrapedJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkills", x => x.JobSkillId);
                    table.ForeignKey(
                        name: "FK_JobSkills_ScrapedJobs_ScrapedJobId",
                        column: x => x.ScrapedJobId,
                        principalTable: "ScrapedJobs",
                        principalColumn: "ScrapedJobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobPreferences",
                columns: table => new
                {
                    JobPreferencesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferredJobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PreferredLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MinimumSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MinimumExperienceYears = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPreferences", x => x.JobPreferencesId);
                    table.ForeignKey(
                        name: "FK_JobPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobRecommendations",
                columns: table => new
                {
                    JobRecommendationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScrapedJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecommendationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRecommendations", x => x.JobRecommendationId);
                    table.ForeignKey(
                        name: "FK_JobRecommendations_ScrapedJobs_ScrapedJobId",
                        column: x => x.ScrapedJobId,
                        principalTable: "ScrapedJobs",
                        principalColumn: "ScrapedJobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobRecommendations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActivities",
                columns: table => new
                {
                    UserActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => x.UserActivityId);
                    table.ForeignKey(
                        name: "FK_UserActivities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    profileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    experienceLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    currentJobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    resumeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.profileId);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Resumes_resumeId",
                        column: x => x.resumeId,
                        principalTable: "Resumes",
                        principalColumn: "resumeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkills",
                columns: table => new
                {
                    skillsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    skillsSkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkills", x => x.skillsId);
                    table.ForeignKey(
                        name: "FK_UserSkills_Skills_skillsSkillId",
                        column: x => x.skillsSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkills_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobPreferences_UserId",
                table: "JobPreferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRecommendations_ScrapedJobId",
                table: "JobRecommendations",
                column: "ScrapedJobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRecommendations_UserId",
                table: "JobRecommendations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_ScrapedJobId",
                table: "JobSkills",
                column: "ScrapedJobId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeSkills_ResumeId",
                table: "ResumeSkills",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserId",
                table: "UserActivities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_resumeId",
                table: "UserProfiles",
                column: "resumeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_userId",
                table: "UserProfiles",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_skillsSkillId",
                table: "UserSkills",
                column: "skillsSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_userId",
                table: "UserSkills",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobPreferences");

            migrationBuilder.DropTable(
                name: "JobRecommendations");

            migrationBuilder.DropTable(
                name: "JobSkills");

            migrationBuilder.DropTable(
                name: "ResumeSkills");

            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UserSkills");

            migrationBuilder.DropTable(
                name: "ScrapedJobs");

            migrationBuilder.DropTable(
                name: "Resumes");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
