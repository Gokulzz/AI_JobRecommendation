using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Models;

namespace app.DAL.Repository
{
    public interface IUserProfileRepository : IGenericRepository<UserProfile>
    {
        public Task<Guid> GetUserProfileId(Guid userId);

    }
}
