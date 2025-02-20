using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.BLL.Services
{
    public interface IRedisService
    {
        public Task EnqueueUserIdAsync(Guid userId);
        public Task<Guid> DequeueUserIdAsync();
    }
}
