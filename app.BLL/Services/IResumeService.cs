using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;

namespace app.BLL.Services
{
    public interface IResumeService
    {
        public Task<ApiResponse> GetResume(Guid id);
        public Task<ApiResponse> AddResume(ResumeDTO resumeDTO);
        public Task<ApiResponse> UpdateResume(Guid resumeId, ResumeDTO resumeDTO);
        public Task<ApiResponse> DeleteResume(Guid resumeId);
    }
}
