using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Application.Responses
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string? Detail { get; }

        public AppException(
            HttpStatusCode statusCode,
            string message,
            string? detail = null)
            : base(message)
        {
            StatusCode = statusCode;
            Detail = detail;
        }
    }
}
