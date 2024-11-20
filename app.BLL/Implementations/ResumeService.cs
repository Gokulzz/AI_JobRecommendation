using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using app.BLL.DTO;
using app.BLL.Exceptions;
using app.BLL.Services;
using app.DAL.Models;
using app.DAL.Repository;
using Aspose.Pdf.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata;
using MimeKit.Encodings;

namespace app.BLL.Implementations
{
    public class ResumeService : IResumeService
    {
        private readonly IUnitofWork unitofWork;
        private readonly IWebHostEnvironment webHost;
        private readonly IUserService userService;
        private readonly IUserProfileService userProfileService;
        public ResumeService(IUnitofWork unitofWork, IWebHostEnvironment webHost, IUserService userService, IUserProfileService userProfileService)     
        {
            this.unitofWork = unitofWork;
            this.webHost = webHost;
            this.userService = userService;
            this.userProfileService = userProfileService;
        }   
        public async Task<ApiResponse> GetResume(Guid id)
        {
            try
            {
                var get_Resume= await unitofWork.ResumeSkillRepository.GetResumeSkills(id); 
                if(get_Resume == null)
                {
                    throw new NotFoundException("Could not find resume");
                }
                return new ApiResponse(200, $"Resume of {id} returned successfully", get_Resume);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
       
        public async Task<ApiResponse> AddResume(ResumeDTO resume)
        {
            try
            {
                var userId = userService.GetCurrentId();
                var userProfileId = await unitofWork.UserProfileRepository.GetUserProfileId(userId);
                var path = webHost.WebRootPath;
                var filePath = "Resumes/" + resume.fileData.FileName;
                var fullPath = Path.Combine(path, filePath);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await resume.fileData.CopyToAsync(fileStream);
                }
                var parsedFields = ExtractFieldsFromText(fullPath);
                var parsed_Summary = parsedFields.ContainsKey("Summary") ? parsedFields["Summary"] : "Not provided";
                var parsed_Education = parsedFields.ContainsKey("Education") ? parsedFields["Education"] : "Not provided";
                var parsed_Skills = parsedFields.ContainsKey("Skills") ? parsedFields["Skills"] : "Not provided";
 
                var add_Resume = new Resume()
                {
                    userProfileId= userProfileId,
                    filePath = filePath,
                    uplodedDate = DateTime.Now,
                    summary = parsed_Summary,
                    Education = parsed_Education,
                    workExperience = "NA",
                    certifications = "NA"
                };
                var resumeSkills = new ResumeSkill()
                {
                    ResumeId = add_Resume.resumeId,
                    SkillName = parsed_Skills
                };
                await unitofWork.ResumeRepository.PostAsync(add_Resume);
                await Update_UserProfile(userProfileId, add_Resume.resumeId);
                await unitofWork.ResumeSkillRepository.PostAsync(resumeSkills);
                await unitofWork.Save();
                return new ApiResponse(200, "Resume uploaded successfully", add_Resume);
            }
            catch(Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }

        }
        public async Task<ApiResponse> UpdateResume(Guid id, ResumeDTO resume)
        {
            throw new NotImplementedException();
        }
        public async Task<ApiResponse> Update_UserProfile(Guid userProfileId, Guid resumeId)
        {
            var get_UserProfile = await unitofWork.UserProfileRepository.GetAsync(userProfileId);
            if(get_UserProfile == null)
            {
                throw new NotFoundException("Could not find the profile for this user");
            }
            get_UserProfile.resumeId= resumeId;
            await unitofWork.Save();
            return new ApiResponse(200, $"Added the resumeId for this  userProfile", resumeId);
        }
        public async Task<ApiResponse> DeleteResume(Guid resumeId)
        {
          var delete_Resume= await unitofWork.ResumeRepository.DeleteAsync(resumeId);
          if(delete_Resume==null)
          {
            throw new NotFoundException("Could not found the resume");
          }
            return new ApiResponse(200, "Resume deleted successfully", delete_Resume);
          
        }
        public static Dictionary<string, string> ExtractFieldsFromText(string fullPath)
        {
            var document = new Aspose.Pdf.Document(fullPath);
            var textAbsorber = new Aspose.Pdf.Text.TextAbsorber();
            document.Pages.Accept(textAbsorber);
            string fullText = textAbsorber.Text;
            var fields = new Dictionary<string, string>();
            fullText = Regex.Replace(fullText, @"\s+", " ").Trim();

            // Adjust regex to match "EXPERIENCE" case-sensitive
            var summaryMatch = Regex.Match(fullText, @"SUMMARY\s*(.*?)\s*(?=(?:EXPERIENCE\b|EDUCATION\b|CERTIFICATIONS\b|PROJECTS\b|SKILLS\b|$))", RegexOptions.Singleline);
            var educationMatch = Regex.Match(fullText, @"EDUCATION\s*(.*?)\s*(?=(?:CERTIFICATIONS|EXPERIENCE|$))", RegexOptions.Singleline);
            var skillsMatch = Regex.Match(fullText, @"SKILLS\s*(.*?)\s*(?=(?:CERTIFICATIONS\b|PROJECTS\b|EXPERIENCE\b|EDUCATION\b|$))", RegexOptions.Singleline);
            if (summaryMatch.Success)
            {
                // Clean extracted summary text
                fields["Summary"] = Regex.Replace(summaryMatch.Groups[1].Value.Trim(), @"\s+", " ");
            }

            if (educationMatch.Success)
            {
                // Clean extracted education text
                fields["Education"] = Regex.Replace(educationMatch.Groups[1].Value.Trim(), @"\s+", " ");
            }
            if (skillsMatch.Success)
            {
                fields["Skills"] = Regex.Replace(skillsMatch.Groups[1].Value.Trim(), @"\s+", " ");
            }

            return fields;
        }






    }
}
