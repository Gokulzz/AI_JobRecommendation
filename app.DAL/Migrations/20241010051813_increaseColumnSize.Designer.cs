﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using app.DAL.Data;

#nullable disable

namespace app.DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241010051813_increaseColumnSize")]
    partial class increaseColumnSize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("app.DAL.Models.JobPreferences", b =>
                {
                    b.Property<Guid>("JobPreferencesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyType")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Industry")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("JobType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("MinimumExperienceYears")
                        .HasColumnType("int");

                    b.Property<decimal?>("MinimumSalary")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("PreferredJobTitle")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PreferredLocation")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("JobPreferencesId");

                    b.HasIndex("UserId");

                    b.ToTable("JobPreferences");
                });

            modelBuilder.Entity("app.DAL.Models.JobRecommendation", b =>
                {
                    b.Property<Guid>("JobRecommendationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Notes")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("RecommendationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ScrapedJobId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("JobRecommendationId");

                    b.HasIndex("ScrapedJobId");

                    b.HasIndex("UserId");

                    b.ToTable("JobRecommendations");
                });

            modelBuilder.Entity("app.DAL.Models.JobSkill", b =>
                {
                    b.Property<Guid>("JobSkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("ScrapedJobId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SkillName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("JobSkillId");

                    b.HasIndex("ScrapedJobId");

                    b.ToTable("JobSkills");
                });

            modelBuilder.Entity("app.DAL.Models.Resume", b =>
                {
                    b.Property<Guid>("resumeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Education")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("certifications")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("filePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("uplodedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("userProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("workExperience")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("resumeId");

                    b.ToTable("Resumes");
                });

            modelBuilder.Entity("app.DAL.Models.ResumeSkill", b =>
                {
                    b.Property<Guid>("ResumeSkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProficiencyLevel")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("ResumeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SkillName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("YearsOfExperience")
                        .HasColumnType("int");

                    b.HasKey("ResumeSkillId");

                    b.HasIndex("ResumeId");

                    b.ToTable("ResumeSkills");
                });

            modelBuilder.Entity("app.DAL.Models.ScrapedJobs", b =>
                {
                    b.Property<Guid>("ScrapedJobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("JobType")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("PostedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Salary")
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("ScrapedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceUrl")
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ScrapedJobId");

                    b.ToTable("ScrapedJobs");
                });

            modelBuilder.Entity("app.DAL.Models.Skill", b =>
                {
                    b.Property<Guid>("SkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProficiencyLevel")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SkillName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("YearsOfExperience")
                        .HasColumnType("int");

                    b.HasKey("SkillId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("app.DAL.Models.User", b =>
                {
                    b.Property<Guid>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("passwordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("passwordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("verfiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("verfificationToken")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("userId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("app.DAL.Models.UserActivity", b =>
                {
                    b.Property<Guid>("UserActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ActivityDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ActivityType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserActivityId");

                    b.HasIndex("UserId");

                    b.ToTable("UserActivities");
                });

            modelBuilder.Entity("app.DAL.Models.UserProfile", b =>
                {
                    b.Property<Guid>("profileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ProfileCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("currentCompany")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("currentJobTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("experienceLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("resumeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("userId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("profileId");

                    b.HasIndex("resumeId")
                        .IsUnique()
                        .HasFilter("[resumeId] IS NOT NULL");

                    b.HasIndex("userId")
                        .IsUnique();

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("app.DAL.Models.UserSkills", b =>
                {
                    b.Property<Guid>("skillsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("skillsSkillId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("userId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("skillsId");

                    b.HasIndex("skillsSkillId");

                    b.HasIndex("userId");

                    b.ToTable("UserSkills");
                });

            modelBuilder.Entity("app.DAL.Models.JobPreferences", b =>
                {
                    b.HasOne("app.DAL.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("app.DAL.Models.JobRecommendation", b =>
                {
                    b.HasOne("app.DAL.Models.ScrapedJobs", "ScrapedJob")
                        .WithMany()
                        .HasForeignKey("ScrapedJobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("app.DAL.Models.User", "User")
                        .WithMany("jobRecommendations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ScrapedJob");

                    b.Navigation("User");
                });

            modelBuilder.Entity("app.DAL.Models.JobSkill", b =>
                {
                    b.HasOne("app.DAL.Models.ScrapedJobs", "ScrapedJob")
                        .WithMany("jobSkills")
                        .HasForeignKey("ScrapedJobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ScrapedJob");
                });

            modelBuilder.Entity("app.DAL.Models.ResumeSkill", b =>
                {
                    b.HasOne("app.DAL.Models.Resume", "Resume")
                        .WithMany()
                        .HasForeignKey("ResumeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resume");
                });

            modelBuilder.Entity("app.DAL.Models.UserActivity", b =>
                {
                    b.HasOne("app.DAL.Models.User", "User")
                        .WithMany("activities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("app.DAL.Models.UserProfile", b =>
                {
                    b.HasOne("app.DAL.Models.Resume", "resume")
                        .WithOne("userProfile")
                        .HasForeignKey("app.DAL.Models.UserProfile", "resumeId");

                    b.HasOne("app.DAL.Models.User", "user")
                        .WithOne("userProfile")
                        .HasForeignKey("app.DAL.Models.UserProfile", "userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("resume");

                    b.Navigation("user");
                });

            modelBuilder.Entity("app.DAL.Models.UserSkills", b =>
                {
                    b.HasOne("app.DAL.Models.Skill", null)
                        .WithMany()
                        .HasForeignKey("skillsSkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("app.DAL.Models.User", null)
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("app.DAL.Models.Resume", b =>
                {
                    b.Navigation("userProfile")
                        .IsRequired();
                });

            modelBuilder.Entity("app.DAL.Models.ScrapedJobs", b =>
                {
                    b.Navigation("jobSkills");
                });

            modelBuilder.Entity("app.DAL.Models.User", b =>
                {
                    b.Navigation("activities");

                    b.Navigation("jobRecommendations");

                    b.Navigation("userProfile")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
