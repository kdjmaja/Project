using System;
using System.Runtime.Serialization;

namespace LibraryWebApp.Models
{
    [Serializable]
    internal class DuplicateBookItemException : Exception
    {
        public DuplicateBookItemException()
        {
        }

        public DuplicateBookItemException(string message) : base(message)
        {
        }

        public DuplicateBookItemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateBookItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}