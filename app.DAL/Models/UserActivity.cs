using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class UserActivity
    {
        [Key]
        public Guid UserActivityId=Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string ActivityType { get; set; }  

        public string? Details { get; set; }

        public DateTime ActivityDate { get; set; } 
    }
}
