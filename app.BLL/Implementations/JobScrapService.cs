using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using app.BLL.DTO;
using app.BLL.Exceptions;
using app.BLL.Services;
using app.DAL.Models;
using app.DAL.Repository;
using Microsoft.AspNetCore.Mvc;

namespace app.BLL.Implementations
{
    public class JobScrapService : IJobScrapService
    {
        private readonly HttpClient _httpClient;
        private readonly IUnitofWork _unitofWork;
        private readonly IUserService _userService;

        public JobScrapService(HttpClient httpClient, IUnitofWork unitofWork, IUserService userService)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(120);
            _httpClient = httpClient;
            _unitofWork = unitofWork;
            _userService = userService;
        }
        public async Task<ApiResponse> GetScrapJobs()
        {
            var get_ScrapedJobs = await _unitofWork.ScrapedJobsRepository.GetScrapedJobswithSkills();
            if(get_ScrapedJobs==null)
            {
                throw new NotFoundException("Scraped jobs could not be found in the system");
            }
            return new ApiResponse(200, "Scraped Jobs returned successfully", get_ScrapedJobs);
        }

        public async Task<ApiResponse> ScrapJobs(Guid userId)
        {
            var jobPreferences= await _unitofWork.JobPreferencesRepository.GetTitleAndLocation(userId);
            if (string.IsNullOrWhiteSpace(jobPreferences.PreferredJobTitle) || string.IsNullOrWhiteSpace(jobPreferences.PreferredLocation))
            {
                throw new BadRequestException("Job title and location are required.");
            }

            var jsonContent = JsonSerializer.Serialize(jobPreferences);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5000/scrape_jobs", content);

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse((int)response.StatusCode, "Error while scraping jobs", await response.Content.ReadAsStringAsync());
            }

            var jobLinksJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jobLinksJson); // Log the JSON to check its structure

            var scrapedJobs = new List<ScrapedJobs>();
            var recommendations = new List<JobRecommendation>();

            foreach (var line in jobLinksJson.Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        var jobResponse = JsonSerializer.Deserialize<ScrapedJobs>(line);

                        if (jobResponse != null)
                        {
                            // Create a new ScrapedJobs object
                            var job = new ScrapedJobs
                            {
                                Title = jobResponse.Title,
                                Company = jobResponse.Company ?? "Unknown",
                                Location = jobResponse.Location ?? "Unknown",
                                SourceUrl = jobResponse.SourceUrl ?? "Unknown",
                                ScrapedDate = DateTime.Now
                            };
                            await _unitofWork.ScrapedJobsRepository.PostAsync(job);

                            // Map job skills from jobResponse to the jobSkill object
                            job.jobSkills = jobResponse.jobSkills.Select(skillName => new JobSkill()
                            {
                                SkillName = skillName.SkillName,
                                ScrapedJobId = job.ScrapedJobId
                            }
                            ).ToList();
                            scrapedJobs.Add(job);
                            await _unitofWork.JobSkillRepository.PostMultiple(job.jobSkills.ToList());
                            

                        //    // Create job recommendations
                        //    var recommendation = new JobRecommendation()
                        //    {
                        //        UserId = _userService.GetCurrentId(),
                        //        ScrapedJobId = job.ScrapedJobId,
                        //        RecommendationDate = DateTime.Now
                        //    };
                        //    recommendations.Add(recommendation);
                        //    await _unitofWork.JobRecommendationsRepository.PostAsync(recommendation);
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error deserializing job data: {ex.Message}");
                    }
                }
            }
             await _unitofWork.Save(); // Save all changes to the database
             return new ApiResponse(200, "Scraped jobs returned successfully", scrapedJobs);
        }
    }
}
