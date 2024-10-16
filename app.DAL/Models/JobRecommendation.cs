using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static app.DAL.Models.ScrapedJobs;

namespace app.DAL.Models
{
    public class JobRecommendation
    {
        [Key]
        public Guid JobRecommendationId=Guid.NewGuid();

        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ScrapedJobId { get; set; }
        public ScrapedJobs ScrapedJob { get; set; }
        public DateTime RecommendationDate { get; set; }  

       [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
