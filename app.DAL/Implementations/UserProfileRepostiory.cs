using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Data;
using app.DAL.Models;
using app.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace app.DAL.Implementations
{
    public class UserProfileRepostiory : GenericRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepostiory(DataContext dataContext): base(dataContext)
        {

        }
        public async Task<Guid> GetUserProfileId(Guid userId)
        {
            try
            {
                var get_userProfileId = await dataContext.UserProfiles.Where(x => x.userId == userId).FirstOrDefaultAsync();
                if(get_userProfileId == null)
                {
                    throw new Exception("User profile could not be found for this user");
                }
                return get_userProfileId.profileId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
