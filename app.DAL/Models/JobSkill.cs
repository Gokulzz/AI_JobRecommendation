using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static app.DAL.Models.ScrapedJobs;

namespace app.DAL.Models
{
    public class JobSkill
    {
        [Key]
        public Guid JobSkillId = Guid.NewGuid();    

        [Required]
        [MaxLength(255)]
        public string SkillName { get; set; } 

        [MaxLength(500)]
        public string? Description { get; set; }

      
        public Guid ScrapedJobId { get; set; }
        public ScrapedJobs ScrapedJob { get; set; }
    }
}
