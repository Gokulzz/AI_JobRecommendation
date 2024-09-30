using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.BLL.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string message) : base(message)
        {

        }
    }
}
