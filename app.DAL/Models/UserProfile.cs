using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class UserProfile
    {
        public Guid profileId= Guid.NewGuid();
        public Guid userId { get; set; }
        public User user { get; set; }  
        public string firstName { get; set; }   
        public  string lastName { get; set; }   
        public string phoneNumber { get; set; }
        public string experienceLevel { get; set; }
        public string? currentJobTitle { get; set; }    
        public string? currentCompany { get; set; } 
        public string Address { get; set; }
        public DateTime? ProfileCreatedAt { get; set; } 
        public Guid resumeId { get; set; }  
        public Resume resume { get; set; }  
        
        
    }
}
