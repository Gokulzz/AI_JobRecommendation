using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using app.BLL.Services;
using app.DAL.Models;
using app.DAL.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;

namespace app.BLL.Implementations
{
    public class JobRecommendationService : IJobRecommendationService
    {
        private readonly IUnitofWork unitofWork;
        private readonly IUserService userService;
        private readonly HttpClient httpClient;
        public JobRecommendationService(IUnitofWork unitofWork, IUserService userService, HttpClient httpClient)
        {
            this.unitofWork = unitofWork;
            this.userService = userService;
            this.httpClient = httpClient;
        }
        public async Task<double> CalculateRelevanceScore(JobPreferences jobPreferences, ScrapedJobs scrapedJobswithSkills,
            string skills)
        {
            var skillsList = Regex.Matches(skills, @"\b[^:,]+(?=\s*(,|$))")
                      .Cast<Match>()
                      .Select(match => match.Value.Trim())
                      .ToList();
            var payload = new
            {
                job_Preferences = new
                {
                    preferred_job_title = jobPreferences.PreferredJobTitle,
                    preferred_location = jobPreferences.PreferredLocation
                },
                scrapedJobs_withSkills = new
                {
                    title = scrapedJobswithSkills.Title,
                    location = scrapedJobswithSkills.Location,
                    job_skills = scrapedJobswithSkills.jobSkills.Select(skill => skill.SkillName).ToList()  
                },
                Skills = skillsList
            };
            //serialize the payload content to be sent to the python server
            var jsonPayload = JsonConvert.SerializeObject(payload);
            Console.WriteLine(jsonPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            //since my flask is running inside the container so using docker built in dns
            var url = "http://localhost:6000/calculate_RelevanceScore";
           //use httpclient to send request to external server with json content type
            var response = await httpClient.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            return JsonConvert.DeserializeObject<double>(result);


        }

        public async Task<ApiResponse> AddJobRecommendation()
        {
            try
            {
                var userId = userService.GetCurrentId();
                var userPreferences = await unitofWork.JobPreferencesRepository.GetTitleAndLocation(userId);
                var getScrapedJobswithSkills = await unitofWork.ScrapedJobsRepository.GetScrapedJobswithSkills();
                var userProfileId = await unitofWork.UserProfileRepository.GetUserProfileId(userId);
                var resumeId = await unitofWork.ResumeRepository.GetResume(userProfileId);
                var userSkills = await unitofWork.ResumeSkillRepository.GetResumeSkills(resumeId);
                var recommendations = new List<JobRecommendation>();

                foreach (var jobs in getScrapedJobswithSkills)
                {
                    double calculateScore =await CalculateRelevanceScore(userPreferences, jobs, string.Join(", ", userSkills));
                    if (calculateScore > 0.05)
                    {
                        var recommendation = new JobRecommendation()
                        {
                            UserId = userId,
                            ScrapedJobId = jobs.ScrapedJobId,
                            RecommendationDate = DateTime.Now,
                            relevanceScore= calculateScore
                        };
                        recommendations.Add(recommendation);
                        await unitofWork.JobRecommendationsRepository.PostAsync(recommendation);
                    }
                }
                await unitofWork.Save();
                return new ApiResponse(200, "Recommendation added successfully", recommendations);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        public async Task<ApiResponse> GetJobRecommendation()
        {
            try
            {
                var userId = userService.GetCurrentId();
                var get_Recommendation = await unitofWork.JobRecommendationsRepository.GetJobRecommendation(userId);
                return new ApiResponse(200, "Job recommendation returned successfully", get_Recommendation);    
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
           
        }
        //private double CalculateRelevanceScore(JobPreferences jobPreferences, ScrapedJobs jobs, string skills)
        //{
        //    double score=0;
        //    if (!string.IsNullOrEmpty(jobPreferences.PreferredJobTitle) && jobs.Title.Contains(jobPreferences.PreferredJobTitle, StringComparison.OrdinalIgnoreCase)) 
        //    {
        //        score += 1;

        //    }
        //    if (!string.IsNullOrEmpty(jobPreferences.PreferredLocation) && jobs.Location.Contains(jobPreferences.PreferredLocation, StringComparison.OrdinalIgnoreCase))
        //    {
        //        score += 1; 
        //    }
        //    if(!string.IsNullOrEmpty(skills)) {
        //        foreach(var skill in skills)
        //        {
        //            if(jobs.jobSkills.Any(js=>js.SkillName.Contains(skill, StringComparison.OrdinalIgnoreCase)))
        //            {
        //                score += 1; 
        //            }
        //        }
                
        //    }
        //    return score;
        //}
    }
}
