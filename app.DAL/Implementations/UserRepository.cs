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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DataContext dataContext) : base(dataContext)
        {

        }
        public async Task<User> VerifyUser(Guid token)
        {
            var search_token = await dataContext.Users.Where(x => x.verfificationToken == token).FirstOrDefaultAsync();
            return search_token;

        }
        public async Task<User> FindUserByEmail(string email)
        {
            var find_email= await dataContext.Users.Where(x=>x.Email==email).FirstOrDefaultAsync();
            return find_email;
        }
    }
}
