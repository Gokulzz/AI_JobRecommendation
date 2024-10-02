﻿using System;
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
            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddValidatorsFromAssemblyContaining<UserValidator>();
           
        }


    }
}
