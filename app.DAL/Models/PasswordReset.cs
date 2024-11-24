using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class PasswordReset
    {
        [Key]
        public Guid Id = Guid.NewGuid();  
        public Guid userId { get; set; }    
        public Guid resetToken { get; set; }    
        public DateTime tokencreatedAt { get; set; }    
        public DateTime tokenExpiresAt { get; set; }
        public bool used { get; set; }
        public User user { get; set; }
    }
}
