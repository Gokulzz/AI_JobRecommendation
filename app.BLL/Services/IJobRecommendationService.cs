using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.BLL.Services
{
    public interface IJobRecommendationService
    {
        public  Task<ApiResponse> AddJobRecommendation();
        public Task<ApiResponse> GetJobRecommendation();
    }
}
