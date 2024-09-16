using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;

namespace app.BLL.Services
{
    public interface IEmailSenderService
    {
        public Task SendEmailAsync(MessageDTO message);
    }
}
