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
        public string userName { get; set; }  
        public string Email { get; set; }
        public byte[] passwordHash { get; set; }   
        public byte[] passwordSalt { get; set; }   
        public string verfificationToken { get; set; }  
        public DateTime? verfiedAt { get; set; }    
        
        
    }
}
