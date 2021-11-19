using System;
using System.Globalization;

namespace Backend.Core.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base() { }

        public ForbiddenException(string message) : base(message) { }

        public ForbiddenException(string message, params object[] args)
            : base(System.String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
