using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace app.DAL.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {

        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.userId);
            modelBuilder.Entity<UserProfile>().HasKey(x => x.profileId);
            modelBuilder.Entity<UserActivity>().HasKey(x => x.UserActivityId);
            modelBuilder.Entity<UserSkills>().HasKey(x => x.skillsId);
            modelBuilder.Entity<Resume>().HasKey(x => x.resumeId);
            modelBuilder.Entity<Skill>().HasKey(x => x.SkillId);
            modelBuilder.Entity<ResumeSkill>().HasKey(x => x.ResumeSkillId);
            modelBuilder.Entity<ScrapedJobs>().HasKey(x => x.ScrapedJobId);
            modelBuilder.Entity<JobSkill>().HasKey(x => x.JobSkillId);
            modelBuilder.Entity<JobRecommendation>().HasKey(x => x.JobRecommendationId);
            modelBuilder.Entity<JobPreferences>().HasKey(x => x.JobPreferencesId);
            modelBuilder.Entity<PasswordReset>().HasKey(x => x.Id);
            modelBuilder.Entity<ScrapedJobs>().Property(x => x.Salary)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<JobPreferences>().Property(x => x.MinimumSalary)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<UserProfile>()
                 .HasOne(x => x.user).WithOne(x => x.userProfile)
                 .HasForeignKey<UserProfile>(x => x.userId);
            modelBuilder.Entity<Resume>()
                .HasOne(x => x.userProfile).WithOne(x => x.resume)
                .HasForeignKey<Resume>(x => x.resumeId);
            modelBuilder.Entity<UserProfile>()
                .HasOne(x => x.resume).WithOne(x => x.userProfile)
                .HasForeignKey<UserProfile>(x => x.resumeId);
            modelBuilder.Entity<User>()
                .HasMany(x => x.skills).WithMany(x => x.users)
                .UsingEntity<UserSkills>();
            modelBuilder.Entity<ScrapedJobs>()
                .HasMany(x => x.jobSkills).WithOne(x => x.ScrapedJob)
                .HasForeignKey(x => x.ScrapedJobId);
            modelBuilder.Entity<User>()
                .HasMany(x => x.jobRecommendations).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>()
                .HasMany(x => x.activities).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>()
                .HasMany(x => x.passwordReset).WithOne(x => x.user)
                .HasForeignKey(x => x.userId);

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; } 
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserSkills> UserSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }    
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<ResumeSkill> ResumeSkills { get; set; }
        public DbSet<ScrapedJobs> ScrapedJobs { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<JobRecommendation> JobRecommendations { get; set; }
        public DbSet<JobPreferences> JobPreferences { get; set; } 
        public DbSet<PasswordReset> PasswordResets { get; set; }
    }
}
