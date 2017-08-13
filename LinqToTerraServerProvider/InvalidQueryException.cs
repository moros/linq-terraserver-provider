using System;

namespace LinqToTerraServerProvider
{
    public class InvalidQueryException : Exception
    {
        private readonly string _message;

        public InvalidQueryException(string message)
        {
            _message = message;
        }

        public override string Message => $"The client query is invalid: {_message}";
    }
}
