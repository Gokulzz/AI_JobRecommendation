using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Data;
using app.DAL.Models;
using app.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace app.DAL.Implementations
{
    public class UnitofWork : IUnitofWork, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IServiceScope _scope;
        private readonly DataContext _context;
        public IUserRepository UserRepository { get; }
        public IUserProfileRepository UserProfileRepository { get; }
        public IUserActivityRepository UserActivityRepository { get; }
        public ISkillRepository SkillRepository { get; }
        public IUserSkillsRepository UserSkillsRepository { get; }
        public IJobSkillRepository JobSkillRepository { get; }
        public IResumeRepository ResumeRepository { get; }
        public IResumeSkillRepository ResumeSkillRepository { get; }
        public IJobPreferencesRepository JobPreferencesRepository { get; }
        public IScrapedJobsRepository ScrapedJobsRepository { get; }
        public IJobRecommendationsRepository JobRecommendationsRepository { get; }
        public IPasswordResetRepository PasswordResetRepository { get; }    
        public UnitofWork(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _scope = _serviceScopeFactory.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<DataContext>();
            UserRepository = new UserRepository(_context);
            UserProfileRepository= new UserProfileRepostiory(_context);
            UserActivityRepository= new UserActivityRepository(_context);   
            SkillRepository= new SkillRepository(_context);
            UserSkillsRepository= new UserSkillsRepository(_context);   
            JobSkillRepository= new JobSkillRepository(_context);
            ResumeRepository= new ResumeRepository(_context);
            ResumeSkillRepository= new ResumeSkillRepository(_context); 
            JobPreferencesRepository= new JobPreferencesRepository(_context);
            ScrapedJobsRepository= new ScrapedJobsRepository(_context);
            JobRecommendationsRepository= new JobRecommendationsRepository(_context);
            PasswordResetRepository= new PasswordResetRepository(_context);

            
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();  
        }
        public async Task<List<string>> GetParsedResumeSkills(Guid userId)
        {
            var resumeSkillsString = await (from resumeSkill in _context.ResumeSkills
                                            join resume in _context.Resumes on resumeSkill.ResumeId equals resume.resumeId
                                            join userProfile in _context.UserProfiles on resume.userProfileId equals userProfile.profileId
                                            join user in _context.Users on userProfile.userId equals user.userId
                                            where user.userId == userId
                                            select resumeSkill.SkillName).FirstOrDefaultAsync();
            //if null return a empty list of string
            if (string.IsNullOrEmpty(resumeSkillsString))
            {
                return new List<string>();
            }

            // Split string by common delimiters
            var skills = resumeSkillsString
                         .Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(skill => skill.Trim())
                         .Where(skill => !string.IsNullOrWhiteSpace(skill))
                         .ToList();

            return skills;
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
