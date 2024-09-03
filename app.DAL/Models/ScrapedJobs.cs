using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class ScrapedJobs
    {
        
            [Key]
            public Guid ScrapedJobId=Guid.NewGuid();

            [Required]
            [MaxLength(255)]
            public string Title { get; set; }  

            [MaxLength(255)]
            public string Company { get; set; } 

            [MaxLength(500)]
            public string Location { get; set; }

            public decimal? Salary { get; set; }  

            [MaxLength(1000)]
            public string Description { get; set; }  

            [MaxLength(500)]
            public string JobType { get; set; }  

            public DateTime PostedDate { get; set; }  

            [Required]
            public string Source { get; set; }  

            public DateTime ScrapedDate { get; set; }  

            [MaxLength(500)]
            public string? SourceUrl { get; set; }  
            public ICollection<JobSkill> jobSkills { get; set; }    
        }

    }

