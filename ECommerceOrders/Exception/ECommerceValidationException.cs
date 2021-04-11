using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ECommerceOrders.Exception
{
    public class ECommerceValidationException : ValidationException
    {
        public HttpStatusCode StatusCode { get; private set; }

        public ECommerceValidationException(string message = "", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
