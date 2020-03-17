using System;
using System.Runtime.Serialization;

namespace Entity.CrossCutting.Exceptions
{
    public class DomainEntityInvariantException : Exception
    {
        public DomainEntityInvariantException() : base() { }
        public DomainEntityInvariantException(string message) : base(message) { }
        public DomainEntityInvariantException(string message, Exception innerException) : base(message, innerException) { }
        public DomainEntityInvariantException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    
}
