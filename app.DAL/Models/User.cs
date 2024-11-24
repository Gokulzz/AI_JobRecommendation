using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class User
    {
        [Key]
        public Guid userId= Guid.NewGuid();
        public UserProfile userProfile { get; set; }    
        public string userName { get; set; }  
        public string Email { get; set; }
        public byte[] passwordHash { get; set; }   
        public byte[] passwordSalt { get; set; }   
        public Guid verfificationToken { get; set; }  
        public DateTime? verfiedAt { get; set; }    
        public ICollection<JobRecommendation>? jobRecommendations { get; set; }
        public ICollection<Skill>? skills { get; set; }
        public ICollection<UserActivity>? activities { get; set; }  
        public ICollection<PasswordReset>? passwordReset { get; set; }




    }
}
