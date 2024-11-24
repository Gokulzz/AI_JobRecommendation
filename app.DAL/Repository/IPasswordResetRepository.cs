using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Models;

namespace app.DAL.Repository
{
    public interface IPasswordResetRepository : IGenericRepository<PasswordReset>
    {
        public Task<PasswordReset?> CheckToken(Guid token);
    }
}
