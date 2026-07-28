using System;
using System.Collections.Generic;
using System.Text;

namespace BELMS.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public string ErrorCode { get; }

        public DomainException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
