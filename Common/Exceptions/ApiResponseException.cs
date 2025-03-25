using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class ApiResponseException : Exception
    {
        public HttpStatusCode StatusCode;

        public ApiResponseException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public ApiResponseException(string message) : base(message)
        {
        }
    }
}
