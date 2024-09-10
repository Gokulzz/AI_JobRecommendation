using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.BLL
{
    public class ApiResponse
    {
        public int statusCode { get; set; } 
        public string message { get; set; } 
        public object result { get; set; }   
        public ApiResponse(int statusCode, string message, object result)
        {
            this.statusCode = statusCode;
            this.message = message;
            this.result = result;
        }   
    }
    
}
