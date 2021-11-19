using System;
using System.Globalization;

namespace Backend.Core.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException() : base() { }

        public ApplicationException(string message) : base(message) { }

        public ApplicationException(string message, params object[] args) : base(System.String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
