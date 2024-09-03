using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class JobPreferences
    {
        [Key]
        public Guid JobPreferencesId=Guid.NewGuid();

       
        public Guid UserId { get; set; }

        public User User { get; set; }

        [MaxLength(100)]
        public string? PreferredJobTitle { get; set; }  

        [MaxLength(50)]
        public string? JobType { get; set; } 

        [MaxLength(255)]
        public string? PreferredLocation { get; set; }  

        [Range(0, 1000000)]
        public decimal? MinimumSalary { get; set; }  

        [MaxLength(100)]
        public string? Industry { get; set; } 

        [MaxLength(100)]
        public string? CompanyType { get; set; } 

        [Range(0, 50)]
        public int? MinimumExperienceYears { get; set; }  
    }
}

