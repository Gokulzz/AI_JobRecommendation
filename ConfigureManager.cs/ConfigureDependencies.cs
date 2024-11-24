using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using app.BLL.Implementations;
using app.BLL.Services;
using app.BLL.Validations;
using app.DAL.Data;
using app.DAL.Implementations;
using app.DAL.Repository;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using app.BLL.DTO;
using System.Text.Json.Serialization;

namespace ConfigureManager.cs
{
    public static class ConfigureDependencies
    {
        public static void ConfigureDependency(this IServiceCollection services)
        {
            services.AddScoped<DataContext>();
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();  
            services.AddScoped<IUserSkillsRepository, UserSkillsRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepostiory>();
            services.AddScoped<IUserActivityRepository, UserActivityRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IScrapedJobsRepository, ScrapedJobsRepository>();
            services.AddScoped<IResumeSkillRepository, ResumeSkillRepository>();
            services.AddScoped<IResumeRepository, ResumeRepository>();
            services.AddScoped<IJobSkillRepository, JobSkillRepository>();
            services.AddScoped<IJobRecommendationsRepository, JobRecommendationsRepository>();
            services.AddScoped<IJobSkillRepository, JobSkillRepository>();
            services.AddScoped<IJobPreferencesRepository, JobPreferencesRepository>();
            services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IResumeService, ResumeService>(); 
            services.AddScoped<IJobScrapService, JobScrapService>();  
            services.AddScoped<IJobRecommendationService, JobRecommendationService>();
            services.AddScoped<IPasswordResetService, PasswordResetService>();  
            services.AddScoped<ICleanUpOldJobsRepository, JobRecommendationsRepository>();
            services.AddScoped<ICleanUpOldJobsRepository, ScrapedJobsRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHostedService<RemoveOldJobService>();
            services.AddHttpClient();
            services.AddValidatorsFromAssemblyContaining<UserValidator>();
            services.AddValidatorsFromAssemblyContaining<PasswordValidator>();
            services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        }


    }
}
