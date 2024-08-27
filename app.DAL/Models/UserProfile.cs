using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class UserProfile
    {
        public Guid profileId= Guid.NewGuid();
        public Guid userId { get; set; }
    }
}
