using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class ScrapedJobs
    {

        [Key]
        public Guid ScrapedJobId = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        [JsonPropertyName("Job Title")]
        public string Title { get; set; }

        [MaxLength(255)]
        [JsonPropertyName("Company")]
        public string Company { get; set; }

        [MaxLength(500)]
        [JsonPropertyName("Location")]
        public string Location { get; set; }

        [MaxLength(1500)]
        [JsonPropertyName("Job Link")]
        public string? SourceUrl { get; set; }

        public decimal? Salary { get; set; }

        [MaxLength(1000)]

        public string? Description { get; set; }

        [MaxLength(500)]
        public string? JobType { get; set; }

        public DateTime? PostedDate { get; set; }


        public string? Source { get; set; }

        public DateTime ScrapedDate { get; set; }
        [JsonPropertyName("Job Skills")]
        [JsonConverter(typeof(JobSkillConverter))]
        public ICollection<JobSkill> jobSkills { get; set; }= new List<JobSkill>(); 


    }
}

