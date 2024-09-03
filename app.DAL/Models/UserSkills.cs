using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class UserSkills
    {
        public Guid userId { get; set; }    
        public Guid skillsId { get; set; }  
    }
}
