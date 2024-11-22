using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace app.BLL.DTO
{
    public class ResumeDTO
    {
        public IFormFile fileData { get; set; }
 
    }
}
