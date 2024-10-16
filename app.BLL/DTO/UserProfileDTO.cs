using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.BLL.DTO
{
    public class UserProfileDTO
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string experienceLevel { get; set; }
        public string? currentJobTitle { get; set; }
        public string? currentCompany { get; set; }
        public string Address { get; set; }
        public string PreferredJobTitle { get; set; }    
        public string PreferredLocation { get; set; }    
      
        
      
    }
}
