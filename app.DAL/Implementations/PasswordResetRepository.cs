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
    public class PasswordResetRepository : GenericRepository<PasswordReset>, IPasswordResetRepository
    {
        public PasswordResetRepository(DataContext dataContext) : base(dataContext)
        {

        }
        public async Task<PasswordReset?> CheckToken(Guid token)
        {
            var check_token = await dataContext.PasswordResets.Where(x=>x.resetToken== token).FirstOrDefaultAsync();    
            return check_token;
        }
    }


    
    
}