using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class Resume
    {
        [Key]
        public Guid resumeId = Guid.NewGuid();
        public Guid userProfileId { get; set; }    
        public UserProfile userProfile { get; set; }  
        public string filePath { get; set; }    
        public DateTime? uplodedDate { get; set; }
        public string summary { get; set; } 
        public string Education { get; set; }   
        public string? workExperience { get; set; }  
        public string? certifications { get; set; } 
    }
}
