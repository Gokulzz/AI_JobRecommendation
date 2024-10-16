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
            _httpClient = httpClient;
            _unitofWork = unitofWork;
            _userService = userService;
        }

        public async Task<ApiResponse> ScrapJobs(JobPreferencesDTO jobPreferencesDTO)
        {
            if (jobPreferencesDTO == null || string.IsNullOrWhiteSpace(jobPreferencesDTO.job_Title) || string.IsNullOrWhiteSpace(jobPreferencesDTO.location))
            {
                throw new BadRequestException("Job title and location are required.");
            }

            var jsonContent = JsonSerializer.Serialize(jobPreferencesDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5000/scrape_jobs", content);

            if (!response.IsSuccessStatusCode)
            {
               return new ApiResponse((int)response.StatusCode, "Error while scraping jobs", await response.Content.ReadAsStringAsync());
            }
            //raw json
            var jobLinksJson = await response.Content.ReadAsStringAsync();
            //add each job to scrapedJobs for structured response
            var scrapedJobs = new List<ScrapedJobs>();
            var recommendations= new List<JobRecommendation>(); 
            foreach (var line in jobLinksJson.Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        var jobResponse = JsonSerializer.Deserialize<ScrapedJobs>(line);
                        if (jobResponse != null)
                        {
                            var job = new ScrapedJobs
                            {
                                Title = jobResponse.Title,
                                Company = jobResponse.Company ?? "Unknown",
                                Location = jobResponse.Location ?? "Unknown",
                                SourceUrl = jobResponse.SourceUrl ?? "Unknown",
                                ScrapedDate = DateTime.Now
                            };
                            scrapedJobs.Add(job);
                            //await _unitofWork.ScrapedJobsRepository.PostAsync(job);
                            var recommendation = new JobRecommendation()
                            {
                                UserId = _userService.GetCurrentId(),
                                ScrapedJobId = job.ScrapedJobId,
                                RecommendationDate = DateTime.Now
                            };
                            recommendations.Add(recommendation);
                            //await _unitofWork.JobRecommendationsRepository.PostAsync(recommendation);
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error deserializing job data: {ex.Message}");
                    }
                }
            }
             //await _unitofWork.Save();
            return new ApiResponse(200, "Scraped jobs returned successfully", scrapedJobs);
        }

    }

}

