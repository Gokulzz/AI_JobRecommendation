using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Repository
{
    public interface IUnitofWork
    {
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
        public Task Save();
    }
}
